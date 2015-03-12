using System;
using System.Diagnostics;

namespace Subterran
{
	internal static class StContract
	{
		[DebuggerHidden]
		public static void ArgumentNotNull(object param, string paramName)
		{
			if (param == null)
				throw new ArgumentNullException(paramName);
		}
	}
}