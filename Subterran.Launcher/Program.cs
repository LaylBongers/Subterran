using System;
using System.IO;
using Subterran.Assets;
using Subterran.WorldState;

namespace Subterran.Launcher
{
	internal static class Program
	{
		private static void Main()
		{
			var gameInfo = GameInfo.FromJson(File.ReadAllText("Game.json"));

			var instance = new GameInstance(gameInfo);
			instance.Run();
		}

		/*private static void Configure(GameInstance instance)
		{
			var window = new StandardWindowService(WindowServiceInfo.FromFile("window.json"));
			instance.AddService<IWindowService>(window);
		}

		[SubterranConfigurator]
		private static void Configure(GameRegistry registry)
		{
			registry.RegisterService<IWindowService, StandardWindowService>();
		}*/
	}

	/*public interface IMyService
	{
	}

	[SubterranService("Standard My Service", new Guid())]
	public class StandardMyService : IMyService
	{
		[SubterranDependency]
		public IAssetService Assets { get; set; }
	}


	[SubterranBehavior("Move Behavior", new Guid())]
	public class MoveBehavior
	{
		[SubterranDependency]
		public IAssetService Assets { get; set; }

		[SubterranEntityDependency]
		public Entity Entity { get; set; }

		[SubterranComponentDependency]
		public PositionComponent Position { get; set; }
	}*/
}