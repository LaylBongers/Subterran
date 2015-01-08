using System;
using System.Drawing;
using OpenTK.Input;

namespace Subterran.Input
{
	public sealed class InputManager
	{
		private readonly Window _window;
		private Point _previousPosition;

		public InputManager(Window window)
		{
			_window = window;
			_window.IsCursorVisible = false;
			_window.ClipCursor = true;
		}

		public event EventHandler<AimEventArgs> AimChange = (e, s) => { };

		public void Update()
		{
			// Get how much the mouse has changed
			var state = Mouse.GetState();
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