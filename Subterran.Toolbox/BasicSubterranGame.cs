using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using OpenTK.Input;
using Subterran.OpenTK;
using Subterran.OpenTK.Diagnostics;

namespace Subterran.Toolbox
{
	public sealed class BasicSubterranGame : Disposable
	{
		private readonly LoopManager _loopManager = new LoopManager();
		private PerformanceCounter _gcTimeCounter;
		private TimeSpan _gcTimeTimer;

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
			_loopManager = new LoopManager();
			_loopManager.Loops.Add(new Loop(Update, 120));
			_loopManager.Loops.Add(new Loop(Render));

			// Set up a performance tracers to warn the developer about stuff
			InitializePerformanceTracers();
		}

		public Window Window { get; set; }

		public InputManager Input { get; set; }

		public Entity World { get; set; }

		public Renderer Renderer { get; set; }

		public Collection<PerformanceTracer> PerformanceTracers { get; set; }

		private void InitializePerformanceTracers()
		{
			_gcTimeCounter = new PerformanceCounter(
				".NET CLR Memory", "% Time in GC",
				Process.GetCurrentProcess().ProcessName);
			float value = 0;

			PerformanceTracers = new Collection<PerformanceTracer>
			{
				new PerformanceTracer(
					() => _loopManager.Loops.Any(l => l.IsRunningSlow),
					() => "The game is running slow!"),
				new PerformanceTracer(
					() =>
					{
						value = _gcTimeCounter.NextValue();
						return value > 10;
					},
					() => "The game has spent a lot of time in garbage collection! (" + value + "%)")
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