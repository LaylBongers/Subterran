using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using Subterran.Rendering;

namespace Subterran.Basic
{
	public abstract class BasicSubterranGame : Disposable
	{
		private readonly LoopManager _loopManager = new LoopManager();
		private TimeSpan _slownessTimer;

		protected BasicSubterranGame(string name)
		{
			// Set up our game loops
			_loopManager = new LoopManager();
			_loopManager.Loops.Add(new Loop(Update, 520));
			_loopManager.Loops.Add(new Loop(_ => Render()));

			// Set up our window and renderer
			Window = new Window(new ScreenSize(1280, 720)) {Title = name};
			Window.Closing += (s, e) => _loopManager.Stop();
			Renderer = new Renderer(Window);

			// Set up the game world
			World = new Entity();
		}

		protected Entity World { get; set; }

		protected Window Window { get; set; }

		protected Renderer Renderer { get; set; }

		public void Run()
		{
			_loopManager.Run();
		}

		protected override void Dispose(bool managed)
		{
		}

		protected virtual void Update(TimeSpan elapsed)
		{
			Window.ProcessEvents();

			// Make sure the developer knows if we're running slow
			if (_loopManager.Loops.Any(l => l.RunningSlow) && _slownessTimer == TimeSpan.Zero)
			{
				Trace.TraceInformation("A loop is running slow!");
				_slownessTimer = TimeSpan.FromSeconds(5);
			}

			// Make sure the timer for the previous warning resets
			_slownessTimer = StMath.Max(_slownessTimer - elapsed, TimeSpan.Zero);
		}

		protected virtual void Render()
		{
			Renderer.Clear(Color.CornflowerBlue);

			Renderer.RenderWorld(World);

			Window.SwapBuffers();
		}
	}
}