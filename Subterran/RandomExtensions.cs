using System;
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
	}
}