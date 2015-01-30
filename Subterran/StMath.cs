using System;
using System.Drawing;
using OpenTK;

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

		public static Vector3 NormalizeColor(Color color)
		{
			return new Vector3(
				NormalizeColor(color.R),
				NormalizeColor(color.G),
				NormalizeColor(color.B));
		}

		public static Vector3 RandomizeColor(Random random, int randomness, Color color)
		{
			return NormalizeColor(Color.FromArgb(
				Range(color.R + random.Next(-randomness, randomness),
					Byte.MinValue, Byte.MaxValue),
				Range(color.G + random.Next(-randomness, randomness),
					Byte.MinValue, Byte.MaxValue),
				Range(color.B + random.Next(-randomness, randomness),
					Byte.MinValue, Byte.MaxValue)));
		}
	}
}