using System;
using JetBrains.Annotations;

namespace Subterran
{
	public sealed class Loop
	{
		private TimeSpan _accumulator;
		private Action<TimeSpan> _callback;
		private TimeSpan _targetDelta;

		private Loop()
		{
		}
		
		public void ExecuteTicks(TimeSpan elapsed)
		{
			// If we don't have a target delta we execute once
			if (_targetDelta == TimeSpan.Zero)
			{
				_callback(elapsed);
				return;
			}

			// Add the time to our internal accumulator, limit it to 4 times the target delta
			_accumulator = StMath.Min(_accumulator + elapsed,
				_targetDelta + _targetDelta + _targetDelta + _targetDelta);

			// Continue till our accumulator is under our target delta
			while (_accumulator >= _targetDelta)
			{
				// Remove our target delta from it
				_accumulator -= _targetDelta;

				// Run our actual tick
				_callback(_targetDelta);
			}
		}

		#region Fluent Interface

		public static Loop ThatCalls(Action<TimeSpan> action)
		{
			return new Loop { _callback = action };
		}

		[Pure]
		public WithRateOfInterface WithRateOf(int amount)
		{
			return new WithRateOfInterface(this, amount);
		}

		public Loop WithDeltaOf(TimeSpan amount)
		{
			_targetDelta = amount;
			return this;
		}

		public class WithRateOfInterface
		{
			private readonly int _amount;
			private readonly Loop _loop;

			public WithRateOfInterface(Loop loop, int amount)
			{
				_loop = loop;
				_amount = amount;
			}

			public Loop PerSecond()
			{
				return _loop.WithDeltaOf(TimeSpan.FromSeconds(1.0/_amount));
			}
		}

		#endregion
	}
}