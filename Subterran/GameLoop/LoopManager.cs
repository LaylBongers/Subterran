using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

namespace Subterran.GameLoop
{
	public sealed class LoopManager
	{
		private bool _keepRunning = true;
		
		/// <summary>
		///     Gets if the instance is currently running.
		/// </summary>
		public bool IsRunning { get; private set; }

		public Collection<GameLoopTimer> Loops { get; } = new Collection<GameLoopTimer>();

		public void Run()
		{
			IsRunning = true;
			var stopwatch = new Stopwatch();

			// Run our main instance loop
			while (_keepRunning)
			{
				// Sanity check to make sure we're not just running 100% CPU with no loops
				if (!Loops.Any())
				{
					throw new InvalidOperationException("Cannot run instance main loop with no loops attached.");
				}

				// Get the elapsed time since last check
				var elapsed = stopwatch.Elapsed;
				stopwatch.Restart();

				// Run the actual tick with the data we've measured
				RunTick(elapsed);
			}

			IsRunning = false;
		}

		private void RunTick(TimeSpan elapsed)
		{
			// Notify all loops of the time passed
			foreach (var loop in Loops)
			{
				loop.ExecuteTicks(elapsed);

				// If we have been told to stop in the run loop, do so immediately
				if (!_keepRunning)
					return;
			}
		}

		/// <summary>
		///     Stops the loop manager's Run() execution.
		/// </summary>
		public void Stop()
		{
			_keepRunning = false;
		}
	}
}