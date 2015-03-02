using System.IO;
using Subterran.Assembling;

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
			using (var game = new Game(gameInfo))
			{
				game.Run();
			}
		}
	}
}