using System;
using System.Globalization;
using System.Linq;

namespace Subterran
{
	public static class StReflection
	{
		public static Type GetTypeFromName(string typeName)
		{
			var types = AppDomain.CurrentDomain
				.GetAssemblies()
				.Select(a => a.GetType(typeName))
				.Where(t => t != null)
				.ToList();

			if (types.Count == 0)
				ThrowTypeError(ExceptionMessages.StReflection_TypeNotFound, typeName);

			if (types.Count > 1)
				ThrowTypeError(ExceptionMessages.StReflection_TypeAmbiguous, typeName);

			return types[0];
		}

		private static void ThrowTypeError(string message, string typeName)
		{
			throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, message, typeName));
		}
	}
}