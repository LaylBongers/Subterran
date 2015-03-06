using System;
using System.Collections.Generic;
using System.Diagnostics;
using Subterran.Assembling;

namespace Subterran.Toolbox
{
	public class BasicEngine
	{
		public BasicEngine(Game game, Dictionary<string, string> args)
		{
			game.AddAssetSource("GameRoot", new DirectoryAssetSource("./"));

			// Get the initial scene
			string startSceneAssetPath;
			if (!args.TryGetValue("StartScene", out startSceneAssetPath))
				throw new InvalidOperationException("StartScene engine parameter is not provided!");

			Trace.TraceInformation("Loading start scene at \"" + startSceneAssetPath + "\"...");
			var scene = game.GetAsset<BasicScene>(startSceneAssetPath);
			Trace.TraceInformation("Loaded scene: " + scene.Name);
		}

		public BasicScene Scene { get; set; }

		[EngineEntryPoint]
		public void Run()
		{
		}
	}
}