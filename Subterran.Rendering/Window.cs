using System;
using System.ComponentModel;
using OpenTK;
using OpenTK.Graphics;

namespace Subterran.Rendering
{
	public class Window
	{
		private readonly GameWindow _window;

		public Window(int width, int height)
		{
			_window = new GameWindow(
				width, height,
				new GraphicsMode(32, 16, 0, 0), // Deferred rendering so no samples
				"Subterran",
				GameWindowFlags.FixedWindow)
			{
				Visible = true,
				VSync = VSyncMode.Adaptive
			};
			_window.Closing += OnClosing;
		}

		public string Title
		{
			get { return _window.Title; }
			set { _window.Title = value; }
		}

		public event EventHandler Closing = (s, e) => { };

		public void ProcessEvents()
		{
			_window.ProcessEvents();
		}

		public void SwapBuffers()
		{
			_window.SwapBuffers();
		}

		public void MakeCurrent()
		{
			if (!_window.Context.IsCurrent)
			{
				_window.MakeCurrent();
			}
		}

		private void OnClosing(object sender, CancelEventArgs args)
		{
			Closing(this, EventArgs.Empty);
		}
	}
}