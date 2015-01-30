namespace Platformer
{
	internal static class Program
	{
		private static void Main()
		{
			using (var game = Platformer.Create())
			{
				game.Run();
			}
		}
	}
}