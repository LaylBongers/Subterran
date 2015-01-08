using System;
using System.Collections.ObjectModel;
using System.Drawing;
using OpenTK.Input;
using Subterran.OpenTK;

namespace Subterran.Toolbox
{
	public sealed class BasicSubterranGame : Disposable
	{
		private readonly LoopManager _loopManager = new LoopManager();

		public BasicSubterranGame(string name)
		{
			// Set up our window
			Window = new Window(new Size(1280, 720)) {Title = name};
			Window.Closing += (s, e) => _loopManager.Stop();

			// Set up our engine's modules
			Input = new InputManager(Window);
			Renderer = new Renderer(Window);

			// Set up the game world
			World = new Entity();

			// Set up our game loops
			_loopManager = new LoopManager
			{
				Loops =
				{
					new Loop(Update, 120),
					new Loop(Render)
				}
			};

			// Set up a performance tracers to warn the developer about stuff
			PerformanceTracers = new Collection<PerformanceTracer>
			{
				PerformanceTracer.CreateLoopSlownessTracer(_loopManager),
				PerformanceTracer.CreateLoopSkippingTracer(_loopManager),
				PerformanceTracer.CreateGcTimeTracer()
			};
		}

		public Window Window { get; set; }

		public InputManager Input { get; set; }

		public Entity World { get; set; }

		public Renderer Renderer { get; set; }

		public Collection<PerformanceTracer> PerformanceTracers { get; set; }

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
			_loopManager.Run();
		}

		public void Stop()
		{
			_loopManager.Stop();
		}

		private void Update(TimeSpan elapsed)
		{
			// Update our window and process any given input
			Window.ProcessEvents();
			Input.Update();

			// Close the game on specific key presses
			// TODO: Make sure OpenTK isn't used by this assembly.
			var keyState = Keyboard.GetState();
			if ( // Alt-F4
				(keyState.IsKeyDown(Key.F4) && keyState.IsKeyDown(Key.AltLeft)) ||
				// Escape
				keyState.IsKeyDown(Key.Escape))
			{
				Stop();
			}

			// Run our developer helper checks
			foreach (var tracer in PerformanceTracers)
			{
				tracer.Update(elapsed);
			}

			// Update the entire world
			World.Call<IUpdatable>(e => e.Update(elapsed));
		}

		private void Render(TimeSpan elapsed)
		{
			Renderer.RenderWorld(World);

			Window.SwapBuffers();
		}
	}
}