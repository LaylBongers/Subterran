using System;
using OpenTK;
using Subterran.Toolbox.Voxels;

namespace ComponentGallery
{
	public static class VoxelMapGenerator
	{
		public static Voxel[,,] GenerateFlat(int size, int height)
		{
			var voxels = new Voxel[size, height, size];

			var random = new Random();

			// Generate the voxel data
			for (var x = 0; x < voxels.GetLength(0); x++)
			{
				for (var z = 0; z < voxels.GetLength(2); z++)
				{
					voxels[x, 0, z].IsSolid = true;
					voxels[x, 0, z].Color = new Vector3(
						(float) random.NextDouble(),
						(float) random.NextDouble(),
						(float) random.NextDouble());
				}
			}

			return voxels;
		}

		public static Voxel[,,] GenerateRandom(int size, int height)
		{
			var voxels = new Voxel[size, height, size];

			var random = new Random();
			var maxHeight = voxels.GetLength(0);

			// Generate the voxel data
			for (var x = 0; x < voxels.GetLength(0); x++)
			{
				for (var z = 0; z < voxels.GetLength(2); z++)
				{
					var columnHeight = random.Next(maxHeight);
					for (var y = 0; y < columnHeight; y++)
					{
						voxels[x, y, z].IsSolid = true;
						voxels[x, y, z].Color = new Vector3(
							(float) random.NextDouble(),
							(float) random.NextDouble(),
							(float) random.NextDouble());
					}
				}
			}

			return voxels;
		}
	}
}