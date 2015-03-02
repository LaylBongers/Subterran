using System.Collections.Generic;
using System.Diagnostics;
using Subterran.Assembling;

namespace Subterran.Toolbox
{
	public class BasicEngine
	{
		public BasicEngine(Dictionary<string, string> args)
		{
			var startScenePath = args["StartScene"];
			Trace.TraceInformation("Start Scene: " + startScenePath);
		}

		//[EngineSceneSection]
		public Entity World { get; set; }

		//[EngineSceneSection]
		public Entity Ui { get; set; }

		[EngineEntryPoint]
		public void Run()
		{
		}
	}
}