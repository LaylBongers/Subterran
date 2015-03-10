using System;
using System.Diagnostics;

namespace Subterran
{
	internal static class StContract
	{
		[DebuggerHidden]
		public static void ArgumentNotNull(object argument, string name)
		{
			if (argument == null)
				throw new ArgumentNullException(name);
		}
	}
}