﻿using System;
using System.ComponentModel;
using OpenTK;
using OpenTK.Graphics;

namespace Subterran.Rendering
{
	public class Window
	{
		private readonly GameWindow _window;

		private Window(int width, int height)
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

		public event EventHandler Closing = (s, e) => { };

		public void ProcessEvents()
		{
			_window.ProcessEvents();
		}

		public void SwapBuffers()
		{
			_window.SwapBuffers();
		}

		private void OnClosing(object sender, CancelEventArgs args)
		{
			Closing(this, EventArgs.Empty);
		}

		#region Fluent Interface

		public Window WithTitle(string title)
		{
			_window.Title = title;
			return this;
		}

		public static Window WithSize(int width, int height)
		{
			return new Window(width, height);
		}

		#endregion
	}
}