using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;

namespace Subterran.GameLoop
{
	public class ClientGameLoop : IGameLoop
	{
		private readonly LoopManager _loopManager;
		private readonly IWindowService _window;

		public ClientGameLoop(GameInstance game, IWindowService window)
		{
			StContract.ArgumentNotNull(game, "game");
			StContract.ArgumentNotNull(window, "window");

			_window = window;
			_window.Title = game.Name;
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

		[SuppressMessage("Microsoft.Reliability", "CA2001:AvoidCallingProblematicMethods")]
		public void Run()
		{
			// Force a full GC collect to avoid lingering stuff causing problems
			// This was added after checking alternatives first and after profiling
			// http://blogs.msdn.com/b/ricom/archive/2004/11/29/271829.aspx
			GC.Collect();
			GC.WaitForPendingFinalizers();
			GC.WaitForFullGCComplete();
			GC.Collect();

			_loopManager.Run();
		}

		public void UpdateTick(TimeSpan elapsed)
		{
			_window.ProcessEvents();
		}

		public void RenderTick(TimeSpan elapsed)
		{
			Thread.Yield();
		}

		private void OnWindowClosing(object sender, EventArgs e)
		{
			_loopManager.Stop();
		}
	}
}