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
				.ThatCalls(Render));

			// Set up our window and renderer
			Window = Window.WithSize(1280, 720).WithTitle(name);
			Window.Closing += (s, e) => _loopManager.Stop();
			Renderer = Renderer.For(Window);
		}

		protected Window Window { get; set; }

		protected Renderer Renderer { get; set; }

		public void Run()
		{
			_loopManager.Run();
		}

		protected override void Dispose(bool managed)
		{
			if (managed)
			{
				Uninitialize();
			}
		}

		protected virtual void Uninitialize()
		{
		}

		protected virtual void Update()
		{
			Window.ProcessEvents();
		}

		protected virtual void Render()
		{
		}
	}
}