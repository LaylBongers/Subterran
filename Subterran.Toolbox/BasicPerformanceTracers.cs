using System.Diagnostics;
using System.Linq;

namespace Subterran.Toolbox
{
	public static class BasicPerformanceTracers
	{
		public static PerformanceTracer CreateLoopSlownessTracer(LoopManager loopManager)
		{
			return new PerformanceTracer(
				() => loopManager.Loops.Any(l => l.IsRunningSlow),
				() => "The game is running slow.");
		}

		public static PerformanceTracer CreateLoopSkippingTracer(LoopManager loopManager)
		{
			return new PerformanceTracer(
				() => loopManager.Loops.Any(l => l.IsSkippingTime),
				() => "The game is skipping frame time.");
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
				() => "The game has spent a lot of time in garbage collection. (" + value + "%)");
		}
	}
}