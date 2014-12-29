using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using OpenTK.Input;
using Subterran.OpenTK;

using Keyboard = OpenTK.Input.Keyboard;

namespace Subterran.Basic
{
	public sealed class BasicSubterranGame : Disposable
	{
		private readonly LoopManager _loopManager = new LoopManager();
		private TimeSpan _slownessTimer;

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
		}

		public Window Window { get; set; }

		public InputManager Input { get; set; }

		public Entity World { get; set; }

		public Renderer Renderer { get; set; }

		protected override void Dispose(bool managed)
		{
			if (managed)
			{
				Input.Dispose();
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
			if (// Alt-F4
				(keyState.IsKeyDown(Key.F4) && keyState.IsKeyDown(Key.AltLeft)) ||
				// Escape
				keyState.IsKeyDown(Key.Escape))
			{
				Stop();
			}

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