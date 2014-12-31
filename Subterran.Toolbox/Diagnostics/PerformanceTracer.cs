using System;
using System.Diagnostics;

namespace Subterran.Toolbox.Diagnostics
{
	public sealed class PerformanceTracer
	{
		private TimeSpan _messageTimer;
		private Func<bool> _condition;
		private Func<string> _warningBuilder;

		public PerformanceTracer(Func<bool> condition, Func<string> warningBuilder)
		{
			_condition = condition;
			_warningBuilder = warningBuilder;
		}

		public void Update(TimeSpan elapsed)
		{
			// Make sure the developer knows if we're running slow
			if (_condition() && _messageTimer == TimeSpan.Zero)
			{
				Trace.TraceInformation(_warningBuilder());
				_messageTimer = TimeSpan.FromSeconds(5);
			}

			// Reduce the timer if it's above TimeSpan.Zero
			_messageTimer = StMath.Max(_messageTimer - elapsed, TimeSpan.Zero);
		}
	}
}