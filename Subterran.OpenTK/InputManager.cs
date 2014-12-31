using System;
using System.Drawing;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Input;

namespace Subterran.OpenTK
{
	public sealed class InputManager : Disposable
	{
		private readonly GameWindow _window;
		private Point _previousPosition;

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
			var state = Mouse.GetState();

			// Get how much the mouse has changed
			var deltaPosition = new Point(
				state.X - _previousPosition.X,
				state.Y - _previousPosition.Y);

			// Reset the mouse to the middle of the screen
			Mouse.SetPosition(
				_window.Bounds.Left + (_window.Bounds.Width / 2),
				_window.Bounds.Top + (_window.Bounds.Height / 2));

			// Store the position of the mouse currently so we can get the delta again next update
			state = Mouse.GetState();
			_previousPosition = new Point(state.X, state.Y);

			// Actually trigger the change event
			AimChange(this, new AimEventArgs(deltaPosition.X, deltaPosition.Y));
		}
	}
}