using System;
using OpenTK;
using SharpNoise.Modules;
using Subterran;
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

		public static Voxel[, ,] GeneratePerlin(int width, int height, int depth)
		{
			var random = new Random();
			var noise = new Perlin { Frequency = 0.01, Seed = random.Next() };
			var map = new Voxel[width, height, depth];

			for (var x = 0; x < width; x++)
			{
				for (var z = 0; z < depth; z++)
				{
					var pillarHeight = StMath.Range((int)((noise.GetValue(x, 0.5, z) + 1) * height * 0.5), 1, height);
					for (var y = 0; y < pillarHeight; y++)
					{
						map[x, y, z] = new Voxel
						{
							IsSolid = true,
							Color = random.NextColor()
						};
					}
				}
			}

			return map;
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