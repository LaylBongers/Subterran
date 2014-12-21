namespace TropicalIsland
{
	internal static class Program
	{
		private static void Main()
		{
			using (var application = TropicalIsland.Create())
			{
				application.Run();
			}
		}
	}
}