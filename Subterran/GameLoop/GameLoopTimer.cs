using System;

namespace Subterran.GameLoop
{
	public sealed class GameLoopTimer
	{
		private readonly Action<TimeSpan> _callback;
		private TimeSpan _accumulator;

		public GameLoopTimer(Action<TimeSpan> callback)
		{
			_callback = callback;
			MaximumTicksPerExecution = 8;
		}

		public GameLoopTimer(Action<TimeSpan> callback, int rate)
			: this(callback)
		{
			_callback = callback;
			TargetDelta = TimeSpan.FromSeconds(1.0/rate);
			MaxDelta = TimeSpan.FromSeconds(TargetDelta.TotalSeconds*3);
		}

		public TimeSpan MaxDelta { get; set; }
		public TimeSpan TargetDelta { get; set; }

		/// <summary>
		/// Gets or sets the maximum amount of ticks allowed per execution of ExecuteTicks.
		/// </summary>
		public int MaximumTicksPerExecution { get; set; }

		public bool IsRunningSlow { get; set; }
		public bool IsSkippingTime { get; set; }

		public void ExecuteTicks(TimeSpan elapsed)
		{
			// If we don't have a target delta we execute once
			if (TargetDelta == TimeSpan.Zero)
			{
				_callback(elapsed);
				return;
			}

			// Add the time to our internal accumulator
			_accumulator = _accumulator + elapsed;

			// Innocent until proven guilty
			IsRunningSlow = false;
			IsSkippingTime = false;
			
			// If we're above the target per tick, we need to adjust it
			var tickDelta = TargetDelta;
			if (_accumulator > TargetDelta.Multiply(MaximumTicksPerExecution))
			{
				tickDelta = _accumulator.Divide(MaximumTicksPerExecution);
				IsRunningSlow = true;

				// Prevent weird jumps caused by big lag spikes
				if (tickDelta > MaxDelta)
				{
					tickDelta = MaxDelta;
					_accumulator = MaxDelta.Multiply(MaximumTicksPerExecution);
					IsSkippingTime = true;
				}
			}

			// Continue till our accumulator is under our target delta
			while (_accumulator >= tickDelta)
			{
				// Remove our target delta from it
				_accumulator -= tickDelta;

				// Run our actual tick
				_callback(tickDelta);
			}
		}
	}
}