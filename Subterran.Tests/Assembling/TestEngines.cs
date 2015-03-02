using System.Collections.Generic;
using Subterran.Assembling;

namespace Subterran.Tests.Assembling
{
	internal class ValidEngine
	{
		public bool WasRun { get; set; }

		public ValidEngine(Dictionary<string, string> args)
		{
		}

		[EngineEntryPoint]
		public void Run()
		{
			WasRun = true;
		}
	}

	internal class InvalidEngine
	{
	}
}