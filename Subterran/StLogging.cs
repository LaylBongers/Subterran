using System.Diagnostics;

namespace Subterran
{
	public class StLogging
	{
		public static void Info(string text)
		{
			Trace.WriteLine(text);
		}
		
		public static void Info(string format, params object[] args)
		{
			Info(string.Format(format, args));
		}
	}
}