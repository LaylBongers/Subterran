﻿using System;
using OpenTK;
using Subterran.Rendering;

namespace Subterran.Toolbox
{
	public class FpsCounterComponent : EntityComponent, IUpdatable, IRenderable
	{
		private readonly Window _window;
		private int _lastSecond;
		private int _renderAmount;
		private int _updateAmount;

		public FpsCounterComponent(Window window)
		{
			_window = window;
		}

		public string Title { get; set; }

		public void Render(Renderer renderer, Matrix4 matrix)
		{
			_renderAmount++;
			CheckTime();
		}

		public void Update(TimeSpan elapsed)
		{
			_updateAmount++;
			CheckTime();
		}

		private void CheckTime()
		{
			var thisSecond = DateTime.Now.Second;
			if (thisSecond == _lastSecond)
				return;

			_window.Title = string.Format("{0} - UFPS: {1}, RFPS: {2}", Title, _updateAmount, _renderAmount);
			_updateAmount = 0;
			_renderAmount = 0;
			_lastSecond = thisSecond;
		}
	}
}