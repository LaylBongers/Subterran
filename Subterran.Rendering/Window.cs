using System;
using System.ComponentModel;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics;

namespace Subterran.Rendering
{
	public sealed class Window
	{
		private readonly GameWindow _window;

		public Window(ScreenSize size)
		{
			_window = new GameWindow(
				size.X, size.Y,
				// Deferred rendering so no samples.
				// If you want AA, it has to be post-process.
				new GraphicsMode(32, 16, 0, 0),
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

		public ScreenSize Size
		{
			get { return new ScreenSize(_window.Width, _window.Height); }
			set { _window.Size = new Size(value.X, value.Y); }
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