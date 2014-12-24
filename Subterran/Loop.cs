using System;

namespace Subterran
{
	public sealed class Loop
	{
		private readonly TimeSpan _accumulationLimit;
		private readonly Action<TimeSpan> _callback;
		private readonly TimeSpan _targetDelta;
		private TimeSpan _accumulator;

		public Loop(Action<TimeSpan> callback)
		{
			_callback = callback;
		}

		public bool IsRunningSlow { get; set; }

		public Loop(Action<TimeSpan> callback, int rate)
			: this(callback)
		{
			_callback = callback;
			_targetDelta = TimeSpan.FromSeconds(1.0/rate);
			_accumulationLimit = TimeSpan.FromSeconds(_targetDelta.TotalSeconds*4);
		}

		public void ExecuteTicks(TimeSpan elapsed)
		{
			// If we don't have a target delta we execute once
			if (_targetDelta == TimeSpan.Zero)
			{
				_callback(elapsed);
				return;
			}

			// Add the time to our internal accumulator
			_accumulator = _accumulator + elapsed;

			// Limit our accumulator to prevent lag spikes from causing weird jumps
			if (_accumulator > _accumulationLimit)
			{
				_accumulator = _accumulationLimit;
				IsRunningSlow = true;
			}
			else
			{
				IsRunningSlow = false;
			}

			// Continue till our accumulator is under our target delta
			while (_accumulator >= _targetDelta)
			{
				// Remove our target delta from it
				_accumulator -= _targetDelta;

				// Run our actual tick
				_callback(_targetDelta);
			}
		}
	}
}