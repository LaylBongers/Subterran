using System;
using System.Threading;

namespace Subterran.GameLoop
{
	public class ClientGameLoop : IGameLoop
	{
		private readonly IWindowService _window;
		private bool _keepRunning;

		public ClientGameLoop(GameInstance game, IWindowService window)
		{
			_window = window;

			_keepRunning = true;
			_window.Title = game.Name;
			_window.Closing += OnWindowClosing;
		}

		public void Run()
		{
			while (_keepRunning)
			{
				_window.ProcessEvents();

				Thread.Sleep(10);
			}
		}

		private void OnWindowClosing(object sender, EventArgs e)
		{
			_keepRunning = false;
		}
	}
}