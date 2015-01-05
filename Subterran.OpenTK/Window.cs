using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics;

namespace Subterran.OpenTK
{
	public sealed class Window : Disposable
	{
		private readonly GameWindow _window;

		public Window(Size size)
		{
			_window = new GameWindow(
				size.Width, size.Height,
				// Deferred rendering so no samples.
				// If you want AA, it has to be post-process.
				new GraphicsMode(32, 16, 0, 0),
				"Subterran",
				GameWindowFlags.FixedWindow)
			{
				Visible = true,
				VSync = VSyncMode.Off
			};
			_window.Closing += OnClosing;
		}

		public string Title
		{
			get { return _window.Title; }
			set { _window.Title = value; }
		}

		public Size Size
		{
			get { return _window.Size; }
			set { _window.Size = value; }
		}

		public bool IsCursorVisible
		{
			get { return _window.CursorVisible; }
			set { _window.CursorVisible = value; }
		}

		public bool Focused
		{
			get { return _window.Focused; }
		}

		public Rectangle Bounds
		{
			get { return _window.Bounds; }
			set { _window.Bounds = value; }
		}

		public event EventHandler Closing = (s, e) => { };

		protected override void Dispose(bool managed)
		{
			if (managed)
			{
				ClearCursorClip();
				_window.Dispose();
			}

			base.Dispose(managed);
		}

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

		public void UpdateCursorClip()
		{
			// TODO: Make this a boolean property and update clip when needed

			var borderSize = (_window.Bounds.Width - _window.ClientSize.Width) / 2;
			Cursor.Clip = new Rectangle(
				_window.Bounds.X + borderSize,
				(_window.Bounds.Y + _window.Bounds.Height) - (_window.ClientSize.Height + borderSize),
				_window.ClientSize.Width,
				_window.ClientSize.Height);
		}

		public void ClearCursorClip()
		{
			Cursor.Clip = Rectangle.Empty;
		}

		private void OnClosing(object sender, CancelEventArgs args)
		{
			Closing(this, EventArgs.Empty);
		}
	}
}