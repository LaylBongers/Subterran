using System.Diagnostics;
using System.Globalization;

namespace Subterran
{
	public static class StLogging
	{
		public static void Info(string text)
		{
			Trace.WriteLine("[Info] " + text);
		}
		
		public static void Info(string format, params object[] args)
		{
			Info(string.Format(CultureInfo.InvariantCulture, format, args));
		}

		public static void Warning(string text)
		{
			Trace.WriteLine("[Warning] " + text);
		}

		public static void Warning(string format, params object[] args)
		{
			Error(string.Format(CultureInfo.InvariantCulture, format, args));
		}

		public static void Error(string text)
		{
			Trace.WriteLine("[Error] " + text);
		}

		public static void Error(string format, params object[] args)
		{
			Error(string.Format(CultureInfo.InvariantCulture, format, args));
		}
	}
}