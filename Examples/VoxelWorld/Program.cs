using System;
using Subterran.Rendering;

namespace VoxelWorld
{
	internal static class Program
	{
		private static void Main()
		{
			try
			{
				using (var game = VoxelWorld.Create())
				{
					game.Run();
				}
			}
			catch (ShaderException ex)
			{
				Console.WriteLine("Shader compile log:\n" + ex.ShaderLog);
				throw;
			}
		}
	}
}