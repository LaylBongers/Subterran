using System;
using System.Drawing;
using Subterran.Rendering;

namespace Subterran.Basic
{
	public abstract class BasicSubterranGame : Disposable
	{
		private readonly LoopManager _loopManager = new LoopManager();

		protected BasicSubterranGame(string name)
		{
			// Set up our game loops
			_loopManager = new LoopManager();
			_loopManager.Loops.Add(Loop
				.ThatCalls(Update)
				.WithRateOf(120).PerSecond());
			_loopManager.Loops.Add(Loop
				.ThatCalls(_ => Render()));

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
		}

		protected virtual void Render()
		{
			Renderer.Clear(Color.CornflowerBlue);

			Renderer.RenderWorld(World);

			Window.SwapBuffers();
		}
	}
}