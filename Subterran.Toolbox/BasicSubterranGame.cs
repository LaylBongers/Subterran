using System;
using System.Collections.ObjectModel;
using System.Drawing;
using OpenTK.Input;
using Subterran.GameLoop;
using Subterran.Rendering;
using Subterran.WorldState;

namespace Subterran.Toolbox
{
	public sealed class BasicSubterranGame : Disposable
	{
		private readonly LoopManager _loopManager;

		public BasicSubterranGame()
		{
			// Set up our window
			Window = new Window(new Size(1280, 720)) {Title = "Subterran"};
			Window.Closing += (s, e) => _loopManager.Stop();

			// Set up our engine's modules
			Renderer = new Renderer(Window);

			// Set up the game world
			World = new Entity();

			// Set up our game loops
			_loopManager = new LoopManager
			{
				Loops =
				{
					new GameLoopTimer(Update, 120),
					new GameLoopTimer(Render)
				}
			};

			// Set up a performance tracers to warn the developer about stuff
			PerformanceTracers = CreatePerformanceTracers();
		}

		public Window Window { get; private set; }
		public Renderer Renderer { get; private set; }
		public Entity World { get; set; }
		public Collection<PerformanceTracer> PerformanceTracers { get; private set; }

		private Collection<PerformanceTracer> CreatePerformanceTracers()
		{
			return new Collection<PerformanceTracer>
			{
				BasicPerformanceTracers.CreateLoopSlownessTracer(_loopManager),
				BasicPerformanceTracers.CreateLoopSkippingTracer(_loopManager),
				// This is commented out because ironically it's causing GC issues, un-comment if needed
				//BasicPerformanceTracers.CreateGcTimeTracer()
			};
		}

		protected override void Dispose(bool managed)
		{
			if (managed)
			{
				Window.Dispose();
			}

			base.Dispose(managed);
		}

		public void Run()
		{
			// Force a full GC collect to avoid lingering stuff causing problems
			GC.Collect();
			GC.WaitForPendingFinalizers();
			GC.WaitForFullGCComplete();
			GC.Collect();

			_loopManager.Run();
		}

		public void Stop()
		{
			_loopManager.Stop();
		}

		private void Update(TimeSpan elapsed)
		{
			EnsureInitialized();

			// Update our window and process any given input
			Window.ProcessEvents();

			// Close the game on specific key presses
			if (IsCloseShortcutDown())
			{
				Stop();
			}

			// Run our developer helper checks
			foreach (var tracer in PerformanceTracers)
			{
				tracer.Update(elapsed);
			}

			// Update the entire world
			World.ForEach<IUpdatable>(e => e.Update(elapsed));
		}

		private static bool IsCloseShortcutDown()
		{
			var keyState = Keyboard.GetState();

			return // Alt-F4
				(keyState.IsKeyDown(Key.F4) && keyState.IsKeyDown(Key.AltLeft)) ||
				// Escape
				keyState.IsKeyDown(Key.Escape);
		}

		private void Render(TimeSpan elapsed)
		{
			EnsureInitialized();

			Renderer.RenderWorld(World);

			Window.SwapBuffers();
		}

		private void EnsureInitialized()
		{
			// Check initialization on all entity components
			World.ForEach<EntityComponent>(c => c.CheckInitialize());
		}
	}
}