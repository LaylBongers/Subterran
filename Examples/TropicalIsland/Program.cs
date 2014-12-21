namespace TropicalIsland
{
	internal static class Program
	{
		private static void Main()
		{
			using (var application = new TropicalIsland())
			{
				application.Run();
			}
		}
	}
}