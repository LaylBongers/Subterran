using System.IO;

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
	}
}