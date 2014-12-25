﻿using System;

namespace Subterran
{
	public static class StMath
	{
		public const float Tau = (float) (Math.PI*2);

		public static TimeSpan Min(TimeSpan val1, TimeSpan val2)
		{
			return val1 < val2 ? val1 : val2;
		}

		public static TimeSpan Max(TimeSpan val1, TimeSpan val2)
		{
			return val1 > val2 ? val1 : val2;
		}
	}
}