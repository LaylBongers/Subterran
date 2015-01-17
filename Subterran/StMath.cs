using System;

namespace Subterran
{
	public static class StMath
	{
		public const float Tau = (float) (Math.PI*2);

		public static TimeSpan Min(TimeSpan left, TimeSpan right)
		{
			return left < right ? left : right;
		}

		public static TimeSpan Max(TimeSpan left, TimeSpan right)
		{
			return left > right ? left : right;
		}

		public static int Range(int value, int min, int max)
		{
			if (value < min) return min;
			if (value > max) return max;

			return value;
		}

		public static float NormalizeColor(int value)
		{
			return (float) value/byte.MaxValue;
		}
	}
}