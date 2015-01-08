using System;
using OpenTK;
using Subterran.Toolbox.Voxels;

namespace ComponentGallery
{
	public static class VoxelMapGenerator
	{
		public static Voxel[,,] GenerateFlat(int size, int height)
		{
			ValidateSizeHeight(size, height);

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
			ValidateSizeHeight(size, height);

			var voxels = new Voxel[size, height, size];
			var random = new Random();

			// Generate the voxel data
			for (var x = 0; x < voxels.GetLength(0); x++)
			{
				for (var z = 0; z < voxels.GetLength(2); z++)
				{
					var columnHeight = random.Next(voxels.GetLength(1));
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

		private static void ValidateSizeHeight(int size, int height)
		{
			if (size <= 0)
				throw new ArgumentOutOfRangeException("size", "Size must be 1 or bigger.");
			if (height <= 0)
				throw new ArgumentOutOfRangeException("height", "Height must be 1 or bigger.");
		}
	}
}