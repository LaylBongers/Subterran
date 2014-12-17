using System.Threading;

namespace TropicalIsland
{
	internal static class Program
	{
		private static void Main()
		{
			// Run our game thread async to avoid window dragging causing the game to stop
			var thread = new Thread(() =>
			{
				var application = new TropicalIsland();
				application.Run();
			}) {Name = "Game Thread"};
			thread.Start();
			thread.Join();
		}
	}
}