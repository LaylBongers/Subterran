using System;
using System.Drawing;
using System.Threading;

namespace Subterran.GameLoop
{
	public class ClientGameLoop : IGameLoop
	{
		private readonly Window _window;
		private bool _keepRunning;

		public ClientGameLoop(GameInstance game)
		{
			_keepRunning = true;
			_window = new Window(new Size(1280, 720));
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