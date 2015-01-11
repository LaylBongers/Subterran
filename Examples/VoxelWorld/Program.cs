namespace VoxelWorld
{
	internal static class Program
	{
		private static void Main()
		{
			using (var game = VoxelWorld.Create())
			{
				game.Run();
			}
		}
	}
}