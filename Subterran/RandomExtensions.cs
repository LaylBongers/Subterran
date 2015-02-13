using System;
using System.Drawing;
using OpenTK;

namespace Subterran
{
	public static class RandomExtensions
	{
		public static float NextFloat(this Random random)
		{
			return (float) random.NextDouble();
		}

		public static Vector3 NextColor(this Random random)
		{
			return new Vector3(
				random.NextFloat(),
				random.NextFloat(),
				random.NextFloat());
		}

		public static Color NextIntColor(this Random random)
		{
			var bytes = new byte[3];
			random.NextBytes(bytes);

			return Color.FromArgb(bytes[0], bytes[1], bytes[2]);
		}

		public static bool NextBool(this Random random)
		{
			return random.Next(0, 2) == 1;
		}
	}
}