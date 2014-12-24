using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

namespace Subterran
{
	public sealed class LoopManager
	{
		private bool _keepRunning = true;

		/// <summary>
		///     Creates and starts a new instance of the Subterran engine.
		/// </summary>
		public LoopManager()
		{
			Loops = new Collection<Loop>();
		}

		/// <summary>
		///     Gets if the instance is currently running.
		/// </summary>
		public bool IsRunning { get; private set; }

		public Collection<Loop> Loops { get; set; }

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
				// If we have been told to stop in another frame, we need to do so
				if (!_keepRunning)
					return;

				loop.ExecuteTicks(elapsed);
			}
		}

		public void Stop()
		{
			_keepRunning = false;
		}
	}
}