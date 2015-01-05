namespace ComponentGallery
{
	internal static class Program
	{
		private static void Main()
		{
			using (var application = ComponentGallery.Create())
			{
				application.Run();
			}
		}
	}
}