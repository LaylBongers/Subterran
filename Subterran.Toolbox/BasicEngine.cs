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

			string startSceneAssetPath;
			if(!args.TryGetValue("StartScene", out startSceneAssetPath))
				throw new InvalidOperationException("StartScene engine parameter is not provided!");

			Trace.TraceInformation("Loading start scene at \"" + startSceneAssetPath + "\"...");
			var scene = game.GetAsset<Scene>(startSceneAssetPath);
			Trace.TraceInformation("Loaded scene: " + scene.Name);
		}

		[EngineSceneSection]
		public Entity World { get; set; }

		[EngineSceneSection]
		public Entity Ui { get; set; }

		[EngineEntryPoint]
		public void Run()
		{
		}
	}
}