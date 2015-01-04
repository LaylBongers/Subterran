using System;

namespace Subterran
{
	public static class RandomExtensions
	{
		public static float NextFloat(this Random random)
		{
			return (float) random.NextDouble();
		}
	}
}