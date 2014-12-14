using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace Subterran
{
	public sealed class StInstance
	{
		private readonly Thread _thread;
		private bool _keepRunning = true;

		/// <summary>
		///     Creates and starts a new instance of the Subterran engine.
		/// </summary>
		/// <param name="initializer">Is called before starting the loops.</param>
		public StInstance(Action<StInstance> initializer)
		{
			Loops = new Collection<Loop>();

			_thread = new Thread(() => Run(initializer)) {Name = "Subterran Thread"};
			_thread.Start();
		}

		/// <summary>
		///     Gets if the instance is currently running.
		/// </summary>
		public bool IsRunning { get; private set; }

		public Collection<Loop> Loops { get; set; }

		/// <summary>
		///     Triggers after existing the main instance loop.
		/// </summary>
		public event EventHandler Uninitialize = (s, e) => { };

		private void Run(Action<StInstance> initializer)
		{
			// Initialize our instance
			initializer(this);

			// Run our main instance loop
			var stopwatch = new Stopwatch();
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

				// Notify all loops of the time passed
				foreach (var loop in Loops)
				{
					loop.ExecuteTicks(elapsed);
				}

				// Use yield to give the rest of our thread's time to another thread
				if (!Thread.Yield())
				{
					// We couldn't yield, sleep a bit instead
					Thread.Sleep(0);
				}
			}

			Uninitialize(this, EventArgs.Empty);
		}

		public void Stop()
		{
			_keepRunning = false;
		}

		public void WaitForStopped()
		{
			_thread.Join();
		}
	}
}