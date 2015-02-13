using System;
using System.Drawing;
using OpenTK;
using SharpNoise;
using SharpNoise.Modules;
using Subterran;
using Subterran.Toolbox.Voxels;

namespace VoxelWorld
{
	internal static class MapGenerator
	{
		private const int Height = 50;
		private const int HeightOffset = 10;
		private static readonly Color Grass = Color.Green;
		private static readonly Color Dirt = Color.SaddleBrown;
		private static readonly Color Stone = Color.Gray;
		private static readonly Color Bedrock = Color.DimGray;

		public static ColoredVoxel[,,] Generate(int width, int depth, Vector2 position)
		{
			var map = new ColoredVoxel[width, Height, depth];

			for (var x = 0; x < width; x++)
			{
				for (var z = 0; z < depth; z++)
				{
					// We want a range of just about 0-1, this won't be always that but usually it is
					var rangedNoise = PerlinNoise.GetValue(x + position.X, 0.5, z + position.Y)*0.5 + 0.5;
					var rawPillarHeight = (rangedNoise*(Height - HeightOffset)) + HeightOffset;
					var pillarHeight = StMath.Range((int) rawPillarHeight, 1, Height);

					GeneratePillar(map, x, z, pillarHeight);
				}
			}

			return map;
		}

		private static void GeneratePillar(ColoredVoxel[,,] map, int x, int z, int height)
		{
			var dirtHeight = Random.Next(2, 5);

			for (var y = 0; y < height; y++)
			{
				map[x, y, z] = new ColoredVoxel
				{
					IsSolid = true,
					Color = StMath.RandomizeColor(Random, 8, GetColorFor(y, height, dirtHeight))
						// Add an effect that higher up is lighter
					        *(((float) y/Height)/2f + 0.5f)
				};
			}
		}

		private static Color GetColorFor(int y, int height, int dirtHeight)
		{
			// Bottom bedrock
			if (y == 0)
				return Bedrock;

			// Higher up diminishing random chance bedrock
			if (y == 1 && Random.Next(0, 2) == 1)
				return Bedrock;
			if (y == 2 && Random.Next(0, 3) == 1)
				return Bedrock;

			// Grass & Dirt layer
			if (y == height - 1)
				return Grass;
			if (y >= height - (dirtHeight + 1))
				return Dirt;

			// Everything else is stone with a random chance of a gem (random color) block
			return Random.Next(0, 50) == 1
				? Random.NextIntColor()
				: Stone;
		}

		private static readonly Random Random = new Random();
		private static readonly int PerlinSeed = Random.Next();

		private static readonly Perlin PerlinNoise = new Perlin
		{
			Frequency = 0.003,
			Seed = PerlinSeed,
			Quality = NoiseQuality.Best
		};
	}
}