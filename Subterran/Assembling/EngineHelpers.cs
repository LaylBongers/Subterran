using System.Collections.Generic;
using System.Reflection;

namespace Subterran.Assembling
{
	internal static class EngineHelpers
	{
		public static bool IsValidConstructor(ConstructorInfo info)
		{
			if (info == null)
				return false;

			if (!info.IsPublic)
				return false;

			var pars = info.GetParameters();
			if (pars.Length != 2)
				return false;

			if (pars[0].ParameterType != typeof(Game))
				return false;
			if (pars[1].ParameterType != typeof(Dictionary<string, string>))
				return false;

			return true;
		}

		public static bool IsValidEntryPoint(MethodInfo info)
		{
			if (info == null)
				return false;

			if (!info.IsPublic)
				return false;

			var pars = info.GetParameters();
			if (pars.Length != 0)
				return false;

			if (info.GetCustomAttribute<EngineEntryPointAttribute>() == null)
				return false;

			return true;
		}
	}
}