using System;
using OpenTK;
using OpenTK.Graphics;
using Subterran;
using Subterran.Toolbox.Voxels;

namespace TropicalIsland
{
	public static class VoxelMapGenerator
	{
		public static Voxel[][][] GenerateFlat(int size, int height)
		{
			var voxels = StArray.CreateJagged<Voxel[][][]>(size, height, size);

			var random = new Random();

			// Generate the voxel data
			for (var x = 0; x < voxels.Length; x++)
			{
				for (var z = 0; z < voxels[x][0].Length; z++)
				{
					voxels[x][0][z].IsSolid = true;
					voxels[x][0][z].Color = new Vector3(
						(float)random.NextDouble(),
						(float)random.NextDouble(),
						(float)random.NextDouble());
				}
			}

			return voxels;
		}

		public static Voxel[][][] GenerateRandom(int size, int height)
		{
			var voxels = StArray.CreateJagged<Voxel[][][]>(size, height, size);

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
						voxels[x][y][z].IsSolid = true;
						voxels[x][y][z].Color = new Vector3(
							(float)random.NextDouble(),
							(float)random.NextDouble(),
							(float)random.NextDouble());
					}
				}
			}

			return voxels;
		}
	}
}