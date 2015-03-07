using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics;

namespace Subterran
{
	public sealed class Window : Disposable
	{
		private readonly GameWindow _window;
		private bool _clipCursor;

		public Window(Size size)
		{
			try
			{
				// Do not use initialization list, it might throw an exception in the list and then it's not yet set
				_window = new GameWindow(
					size.Width, size.Height,
					// Deferred rendering so no samples.
					// If you want AA, it has to be post-process.
					new GraphicsMode(32, 16, 0, 0),
					"Subterran",
					GameWindowFlags.FixedWindow);

				_window.Visible = true;
				_window.VSync = VSyncMode.Adaptive;
				_window.Closing += _window_Closing;
				_window.Resize += _window_ResizeMoveFocus;
				_window.Move += _window_ResizeMoveFocus;
				_window.FocusedChanged += _window_ResizeMoveFocus;
			}
			catch
			{
				_window?.Dispose();
				throw;
			}
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

		public bool ShowCursor
		{
			get { return _window.CursorVisible; }
			set { _window.CursorVisible = value; }
		}

		public bool IsFocused
		{
			get { return _window.Focused; }
		}

		public Rectangle Bounds
		{
			get { return _window.Bounds; }
			set { _window.Bounds = value; }
		}

		public bool ClipCursor
		{
			set
			{
				_clipCursor = value;
				if (_clipCursor && _window.Focused)
				{
					UpdateCursorClip();
				}
				else
				{
					ClearCursorClip();
				}
			}
			get { return _clipCursor; }
		}

		public event EventHandler Closing;

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

		private void UpdateCursorClip()
		{
			var borderSize = (_window.Bounds.Width - _window.ClientSize.Width)/2;
			Cursor.Clip = new Rectangle(
				_window.Bounds.X + borderSize,
				(_window.Bounds.Y + _window.Bounds.Height) - (_window.ClientSize.Height + borderSize),
				_window.ClientSize.Width,
				_window.ClientSize.Height);
		}

		private static void ClearCursorClip()
		{
			Cursor.Clip = Rectangle.Empty;
		}

		private void _window_ResizeMoveFocus(object sender, EventArgs e)
		{
			if (_clipCursor && _window.Focused)
			{
				UpdateCursorClip();
			}
			else
			{
				ClearCursorClip();
			}
		}

		private void _window_Closing(object sender, CancelEventArgs args)
		{
			ClearCursorClip();
			Closing?.Invoke(this, EventArgs.Empty);
		}
	}
}