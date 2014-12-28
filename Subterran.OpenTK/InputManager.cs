using System;
using System.Drawing;
using System.Windows.Forms;
using OpenTK;

namespace Subterran.OpenTK
{
	public sealed class InputManager : Disposable
	{
		private readonly GameWindow _window;
		private Point _currentPointer, _previousPointer;

		public InputManager(Window window)
		{
			_window = window.OpenTkWindow;
			_window.CursorVisible = false;

			ClipCursor();
		}

		protected override void Dispose(bool managed)
		{
			if (managed)
			{
				Cursor.Clip = new Rectangle();
			}

			base.Dispose(managed);
		}

		public event EventHandler<AimEventArgs> AimChange = (e, s) => { };

		private void ClipCursor()
		{
			var borderSize = (_window.Bounds.Width - _window.ClientSize.Width)/2;
			Cursor.Clip = new Rectangle(
				_window.Bounds.X + borderSize,
				(_window.Bounds.Y + _window.Bounds.Height) - (_window.ClientSize.Height + borderSize),
				_window.ClientSize.Width,
				_window.ClientSize.Height);
		}

		public void Update()
		{
			_currentPointer = Cursor.Position;

			var deltaPointer = new Point(
				_currentPointer.X - _previousPointer.X,
				_currentPointer.Y - _previousPointer.Y);

			Cursor.Position = new Point(
				_window.Bounds.Left + (_window.Bounds.Width/2),
				_window.Bounds.Top + (_window.Bounds.Height/2));

			_previousPointer = Cursor.Position;

			AimChange(this, new AimEventArgs(deltaPointer.X, deltaPointer.Y));
		}
	}
}