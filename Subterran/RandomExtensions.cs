using System;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using OpenTK;

namespace Subterran
{
	public static class RandomExtensions
	{
		[SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames")]
		public static float NextFloat(this Random random)
		{
			if (random == null)
				throw new ArgumentNullException("random");

			return (float) random.NextDouble();
		}

		public static Vector3 NextColor(this Random random)
		{
			if (random == null)
				throw new ArgumentNullException("random");

			return new Vector3(
				random.NextFloat(),
				random.NextFloat(),
				random.NextFloat());
		}

		public static Color NextIntColor(this Random random)
		{
			if (random == null)
				throw new ArgumentNullException("random");

			var bytes = new byte[3];
			random.NextBytes(bytes);

			return Color.FromArgb(bytes[0], bytes[1], bytes[2]);
		}

		public static bool NextBoolean(this Random random)
		{
			if(random == null)
				throw new ArgumentNullException("random");

			return random.Next(0, 2) == 1;
		}
	}
}