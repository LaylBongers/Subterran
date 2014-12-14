namespace TropicalIsland
{
	internal static class Program
	{
		private static void Main()
		{
			var game = TropicalIsland.CreateGame();
			game.WaitForStopped();
		}
	}
}