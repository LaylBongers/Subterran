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

		public static ColoredVoxel[,,] Generate(int width, int depth, Vector2 perlinOffset)
		{
			var map = new ColoredVoxel[width, Height, depth];

			for (var x = 0; x < width; x++)
			{
				for (var z = 0; z < depth; z++)
				{
					GeneratePillar(map, x, z, perlinOffset);
				}
			}

			return map;
		}

		private static void GeneratePillar(ColoredVoxel[,,] map, int x, int z, Vector2 perlinOffset)
		{
			// We want a range of just about 0-1, this won't be always that but usually it is
			var rangedNoise = HeightNoise.GetValue(x + perlinOffset.X, 0.5, z + perlinOffset.Y)*0.5 + 0.5;
			var rawPillarHeight = (rangedNoise*(Height - HeightOffset)) + HeightOffset;

			var pillarHeight = StMath.Range((int) rawPillarHeight, 1, Height);
			var dirtHeight = Random.Next(2, 5);

			for (var y = 0; y < pillarHeight; y++)
			{
				map[x, y, z] = GenerateBlock(x, y, z, pillarHeight, dirtHeight, perlinOffset);
			}
		}

		private static ColoredVoxel GenerateBlock(int x, int y, int z, int pillarHeight, int dirtHeight, Vector2 perlinOffset)
		{
			var voxel = new ColoredVoxel {IsSolid = true};

			if (IsBedrock(y))
			{
				voxel.Color = DimForHeight(StMath.RandomizeColor(Random, 8, Bedrock), y);
				return voxel;
			}

			var noise = CaveNoise.GetValue(x + perlinOffset.X, y, z + perlinOffset.Y);
			var isCave = noise > -0.05f && noise < 0.05f;

			if (isCave)
			{
				voxel.IsSolid = false;
				return voxel;
			}

			voxel.Color = DimForHeight(StMath.RandomizeColor(Random, 8, GetColorFor(y, pillarHeight, dirtHeight)), y);

			return voxel;
		}

		private static Vector3 DimForHeight(Vector3 color, int y)
		{
			return color*(((float) y/Height)/2f + 0.5f);
		}

		private static bool IsBedrock(int y)
		{
			// Bottom bedrock
			if (y == 0)
				return true;

			// Higher up diminishing random chance bedrock
			if (y == 1 && Random.Next(0, 2) == 1)
				return true;
			if (y == 2 && Random.Next(0, 3) == 1)
				return true;

			return false;
		}

		private static Color GetColorFor(int y, int height, int dirtHeight)
		{
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

		private static readonly Perlin HeightNoise = new Perlin
		{
			Frequency = 0.003,
			Seed = PerlinSeed,
			Quality = NoiseQuality.Best
		};

		private static readonly Perlin CaveNoise = new Perlin
		{
			Frequency = 0.005,
			Seed = PerlinSeed,
			Quality = NoiseQuality.Best
		};
	}
}