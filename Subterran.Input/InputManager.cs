namespace Subterran.Input
{
	public sealed class InputManager
	{
		private readonly Window _window;

		public InputManager(Window window)
		{
			_window = window;
			_window.ClipCursor = true;
		}

		public bool ShowCursor
		{
			get { return _window.ShowCursor; }
			set { _window.ShowCursor = value; }
		}

		public void Update()
		{
		}
	}
}