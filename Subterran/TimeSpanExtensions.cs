using System;
using OpenTK;

namespace Subterran
{
	public static class TimeSpanExtensions
	{
		public static float PerSecond(this TimeSpan delta, float amountPerSecond)
		{
			return amountPerSecond*(float) delta.TotalSeconds;
		}

		public static Vector3 PerSecond(this TimeSpan delta, Vector3 amountPerSecond)
		{
			return amountPerSecond*(float) delta.TotalSeconds;
		}
	}
}