using System;

namespace TropicalIsland
{
	public static class VoxelMapGenerator
	{
		public static void GenerateFlatIn(bool[][][] voxels)
		{
			for (var x = 0; x < voxels.Length; x++)
			{
				for (var z = 0; z < voxels[x][0].Length; z++)
				{
					voxels[x][0][z] = true;
				}
			}
		}

		public static void GenerateRandomIn(bool[][][] voxels)
		{
			var random = new Random();
			var maxHeight = voxels[0].Length;

			for (var x = 0; x < voxels.Length; x++)
			{
				for (var z = 0; z < voxels[x][0].Length; z++)
				{
					var height = random.Next(maxHeight);
					for (var y = 0; y < height; y++)
					{
						voxels[x][y][z] = true;
					}
				}
			}
		}
	}
}