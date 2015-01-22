using System;
using System.Drawing;
using OpenTK;
using SharpNoise.Modules;
using Subterran;
using Subterran.Toolbox.Voxels;

namespace VoxelWorld
{
	internal static class MapGenerator
	{
		public static ColoredVoxel[,,] Generate(int width, int depth)
		{
			const int height = 50;
			const int offset = 10;

			var random = new Random();
			var noise = new Perlin {Frequency = 0.005, Seed = random.Next()};
			var map = new ColoredVoxel[width, height, depth];

			var grass = Color.Green;
			var dirt = Color.SaddleBrown;
			var stone = Color.Gray;

			for (var x = 0; x < width; x++)
			{
				for (var z = 0; z < depth; z++)
				{
					// We want a range of just about 0-1, this won't be always that but usually it is
					var rangedNoise = noise.GetValue(x, 0.5, z)*0.5 + 0.5f;

					var rawPillarHeight = rangedNoise*(height - offset) + offset;
					var pillarHeight = StMath.Range((int) rawPillarHeight, 1, height);
					var dirtHeight = random.Next(2, 5);

					for (var y = 0; y < pillarHeight; y++)
					{
						Color color;
						if (y == pillarHeight - 1)
							color = grass;
						else if (y >= pillarHeight - (dirtHeight + 1))
							color = dirt;
						else
							color = stone;

						map[x, y, z] = new ColoredVoxel
						{
							IsSolid = true,
							Color = RandomizeColor(random, 10, color)
						};
					}
				}
			}

			return map;
		}

		private static Vector3 RandomizeColor(Random random, int randomness, Color color)
		{
			return new Vector3(
				StMath.NormalizeColor(
					StMath.Range(color.R + random.Next(-randomness, randomness),
						Byte.MinValue, Byte.MaxValue)),
				StMath.NormalizeColor(
					StMath.Range(color.G + random.Next(-randomness, randomness),
						Byte.MinValue, Byte.MaxValue)),
				StMath.NormalizeColor(
					StMath.Range(color.B + random.Next(-randomness, randomness),
						Byte.MinValue, Byte.MaxValue)));
		}
	}
}