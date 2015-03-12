using System;

namespace Subterran
{
	public interface IWindowService
	{
		string Title { get; set; }
		event EventHandler Closing;
		void ProcessEvents();
	}
}