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

		public static float Range(float value, float min, float max)
		{
			if (value < min) return min;
			if (value > max) return max;

			return value;
		}

		public static int Range(int value, int min, int max)
		{
			if (value < min) return min;
			if (value > max) return max;

			return value;
		}

		public static Vector3i Floor(Vector3 value)
		{
			return new Vector3i(
				(int) Math.Floor(value.X),
				(int) Math.Floor(value.Y),
				(int) Math.Floor(value.Z));
		}

		public static Vector3i Ceiling(Vector3 value)
		{
			return new Vector3i(
				(int) Math.Ceiling(value.X),
				(int) Math.Ceiling(value.Y),
				(int) Math.Ceiling(value.Z));
		}

		public static Vector3i Range(Vector3i value, Vector3i min, Vector3i max)
		{
			return new Vector3i(
				Range(value.X, min.X, max.X),
				Range(value.Y, min.Y, max.Y),
				Range(value.Z, min.Z, max.Z));
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
			if(random == null)
				throw new ArgumentNullException("random");

			return NormalizeColor(Color.FromArgb(
				Range(color.R + random.Next(-randomness, randomness),
					Byte.MinValue, Byte.MaxValue),
				Range(color.G + random.Next(-randomness, randomness),
					Byte.MinValue, Byte.MaxValue),
				Range(color.B + random.Next(-randomness, randomness),
					Byte.MinValue, Byte.MaxValue)));
		}

		public static int Round(double value)
		{
			var retVal = (int) value;

			if (value%1 >= 0.5)
				retVal++;

			return retVal;
		}

		public static float AddSigned(float signed, float addition)
		{
			return signed >= 0
				? signed + addition
				: signed - addition;
		}
	}
}