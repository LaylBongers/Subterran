using System;
using System.Diagnostics;
using System.Linq;

namespace Subterran.Toolbox
{
	public sealed class PerformanceTracer
	{
		private readonly Func<bool> _condition;
		private readonly Func<string> _warningBuilder;
		private TimeSpan _messageTimer;

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

		public static PerformanceTracer CreateLoopSlownessTracer(LoopManager loopManager)
		{
			return new PerformanceTracer(
				() => loopManager.Loops.Any(l => l.IsRunningSlow),
				() => "The game is running slow!");
		}

		public static PerformanceTracer CreateLoopSkippingTracer(LoopManager loopManager)
		{
			return new PerformanceTracer(
				() => loopManager.Loops.Any(l => l.IsSkippingTime),
				() => "The game is skipping frame time!");
		}

		public static PerformanceTracer CreateGcTimeTracer()
		{
			var gcTimeCounter = new PerformanceCounter(
				".NET CLR Memory", "% Time in GC",
				Process.GetCurrentProcess().ProcessName);
			float value = 0;

			return new PerformanceTracer(
				() =>
				{
					value = gcTimeCounter.NextValue();
					return value > 10;
				},
				() => "The game has spent a lot of time in garbage collection! (" + value + "%)");
		}
	}
}