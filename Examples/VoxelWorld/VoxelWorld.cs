using Subterran.Toolbox;

namespace VoxelWorld
{
	internal static class VoxelWorld
	{
		public static BasicSubterranGame Create()
		{
			var game = new BasicSubterranGame("Voxel World");

			return game;
		}
	}
}