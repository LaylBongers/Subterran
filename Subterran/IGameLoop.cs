using System;

namespace Subterran
{
	public interface IGameLoop
	{
		event EventHandler Stopped;

		void Run();
		void StopRunning();
	}
}