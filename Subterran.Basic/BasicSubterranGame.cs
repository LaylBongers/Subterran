using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using Subterran.Rendering;

namespace Subterran.Basic
{
	public sealed class BasicSubterranGame : Disposable
	{
		private readonly LoopManager _loopManager = new LoopManager();
		private TimeSpan _slownessTimer;

		public BasicSubterranGame(string name)
		{
			// Set up our game loops
			_loopManager = new LoopManager();
			_loopManager.Loops.Add(new Loop(Update, 120));
			_loopManager.Loops.Add(new Loop(Render));

			// Set up our window and renderer
			Window = new Window(new ScreenSize(1280, 720)) {Title = name};
			Window.Closing += (s, e) => _loopManager.Stop();
			Renderer = new Renderer(Window);

			// Set up the game world
			World = new Entity();
		}

		public Entity World { get; set; }

		public Window Window { get; set; }

		public Renderer Renderer { get; set; }

		public void Run()
		{
			_loopManager.Run();
		}

		protected override void Dispose(bool managed)
		{
			base.Dispose(managed);
		}

		private void Update(TimeSpan elapsed)
		{
			Window.ProcessEvents();

			// Make sure the developer knows if we're running slow
			if (_loopManager.Loops.Any(l => l.IsRunningSlow) && _slownessTimer == TimeSpan.Zero)
			{
				Trace.TraceInformation("The game is running slow!");
				_slownessTimer = TimeSpan.FromSeconds(5);
			}

			// Reduce the timer if it's above TimeSpan.Zero
			_slownessTimer = StMath.Max(_slownessTimer - elapsed, TimeSpan.Zero);

			// Update the entire world
			World.Update(elapsed);
		}

		private void Render(TimeSpan elapsed)
		{
			Renderer.Clear(Color.CornflowerBlue);

			Renderer.RenderWorld(World);

			Window.SwapBuffers();
		}
	}
}