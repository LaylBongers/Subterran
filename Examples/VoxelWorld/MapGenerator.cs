using System;
using SharpNoise.Modules;
using Subterran;
using Subterran.Toolbox.Voxels;

namespace VoxelWorld
{
	internal static class MapGenerator
	{
		public static Voxel[,,] Generate(int width, int height, int depth)
		{
			var random = new Random();
			var noise = new Perlin {Frequency = 0.01, Seed = random.Next()};
			var map = new Voxel[width, height, depth];

			for (var x = 0; x < width; x++)
			{
				for (var z = 0; z < depth; z++)
				{
					var pillarHeight = StMath.Range((int)((noise.GetValue(x, 0.5, z) + 1)*height*0.5), 1, height);
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
	}
}