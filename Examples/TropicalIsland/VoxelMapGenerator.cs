using System;
using Subterran;

namespace TropicalIsland
{
	public static class VoxelMapGenerator
	{
		public static bool[][][] GenerateFlat(int size, int height)
		{
			var voxels = StArray.CreateJagged<bool[][][]>(size, height, size);

			// Generate the voxel data
			for (var x = 0; x < voxels.Length; x++)
			{
				for (var z = 0; z < voxels[x][0].Length; z++)
				{
					voxels[x][0][z] = true;
				}
			}

			return voxels;
		}

		public static bool[][][] GenerateRandom(int size, int height)
		{
			var voxels = StArray.CreateJagged<bool[][][]>(size, height, size);

			var random = new Random();
			var maxHeight = voxels[0].Length;

			// Generate the voxel data
			for (var x = 0; x < voxels.Length; x++)
			{
				for (var z = 0; z < voxels[x][0].Length; z++)
				{
					var columnHeight = random.Next(maxHeight);
					for (var y = 0; y < columnHeight; y++)
					{
						voxels[x][y][z] = true;
					}
				}
			}

			return voxels;
		}
	}
}