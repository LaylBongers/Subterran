using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using OpenTK.Input;
using Subterran.Input;
using Subterran.WorldState;

namespace Subterran.GameLoop
{
	public sealed class ClientGameLoop : IGameLoop
	{
		private readonly IInputService _input;
		private readonly LoopManager _loopManager;
		private readonly IWindowService _window;
		private readonly IWorldStateService _world;

		public ClientGameLoop(IWindowService window, IWorldStateService world, IInputService input)
		{
			StContract.ArgumentNotNull(window, "window");
			StContract.ArgumentNotNull(world, "world");
			StContract.ArgumentNotNull(input, "input");

			_window = window;
			_world = world;
			_input = input;
			
			_window.Closing += OnWindowClosing;
			
			// Set up our game loops
			_loopManager = new LoopManager
			{
				Loops =
				{
					new GameLoopTimer(UpdateTick, 120),
					new GameLoopTimer(RenderTick)
				}
			};
		}

		public event EventHandler Stopped;

		[SuppressMessage("Microsoft.Reliability", "CA2001:AvoidCallingProblematicMethods")]
		public void Run()
		{
			// Everything's ready, set up the world
			_world.LoadDefault();

			// Force a full GC collect to avoid lingering stuff causing problems
			// This was added after checking alternatives first and after profiling
			// http://blogs.msdn.com/b/ricom/archive/2004/11/29/271829.aspx
			GC.Collect();
			GC.WaitForPendingFinalizers();
			GC.WaitForFullGCComplete();
			GC.Collect();

			_loopManager.Run();
		}

		public void StopRunning()
		{
			_loopManager.Stop();
			Stopped?.Invoke(this, EventArgs.Empty);
		}

		public void UpdateTick(TimeSpan elapsed)
		{
			_window.ProcessEvents();

			// Close the game on specific key presses
			if (IsCloseShortcutDown())
				StopRunning();
		}

		public void RenderTick(TimeSpan elapsed)
		{
			Thread.Yield();
		}

		private bool IsCloseShortcutDown()
		{
			return // Alt-F4
				(_input.IsKeyDown(Key.F4) && _input.IsKeyDown(Key.AltLeft)) ||
				// Escape
				_input.IsKeyDown(Key.Escape);
		}

		private void OnWindowClosing(object sender, EventArgs e)
		{
			_loopManager.Stop();
		}
	}
}