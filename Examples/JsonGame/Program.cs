using System.IO;
using Subterran;

namespace JsonGame
{
	internal static class Program
	{
		private static void Main()
		{
			Subterran.Toolbox.Dummy.ForceCopy();

			// Read in the game's info from the game project file
			var gameInfo = GameInfo.FromJson(File.ReadAllText("./Game.json"));

			// Create and run our actual game
			var game = new GameInstance(gameInfo);
			game.Run();
		}
	}
}