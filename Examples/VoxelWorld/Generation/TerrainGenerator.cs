using System;
using OpenTK;
using SharpNoise;
using SharpNoise.Modules;
using Subterran;
using Subterran.Toolbox.Voxels;
using VoxelWorld.Voxels;

namespace VoxelWorld.Generation
{
	public static class TerrainGenerator
	{
		public static void Generate(TexturedVoxel[,,] map, Vector2 perlinOffset, int heightOffset)
		{
			var width = map.GetLength(0);
			var height = map.GetLength(1);
			var depth = map.GetLength(2);

			for (var x = 0; x < width; x++)
			{
				for (var z = 0; z < depth; z++)
				{
					GeneratePillar(map, x, z, perlinOffset, height, heightOffset);
				}
			}
		}

		private static void GeneratePillar(TexturedVoxel[,,] map, int x, int z, Vector2 perlinOffset, int height,
			int heightOffset)
		{
			// We want a range of just about 0-1, this won't be always that but usually it is
			var rawNoise = (float) HeightNoise.GetValue(x + perlinOffset.X, 0.5, z + perlinOffset.Y)*0.5f + 0.5f;
			var rawPillarHeight = (StMath.Range(rawNoise, 0.0f, 1.0f)*(height - heightOffset)) + heightOffset;

			var pillarHeight = StMath.Range((int) rawPillarHeight, 1, height);
			var dirtHeight = Random.Next(2, 5);

			for (var y = 0; y < pillarHeight; y++)
			{
				map[x, y, z] = GenerateBlock(x, y, z, pillarHeight, dirtHeight, perlinOffset);
			}
		}

		private static TexturedVoxel GenerateBlock(int x, int y, int z, int pillarHeight, int dirtHeight, Vector2 perlinOffset)
		{
			var voxel = new TexturedVoxel();

			// Bedrock overrides everything, it NEEDS to be there
			if (IsBedrock(y))
			{
				voxel.Type = VoxelTypes.Bedrock;
				return voxel;
			}

			var noise = CaveNoise.GetValue(x + perlinOffset.X, y, z + perlinOffset.Y);
			var isCave = noise > -0.05f && noise < 0.05f;

			if (isCave)
			{
				return voxel;
			}

			voxel.Type = GetTypeFor(y, pillarHeight, dirtHeight);

			return voxel;
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

		private static ITexturedVoxelType GetTypeFor(int y, int height, int dirtHeight)
		{
			// Grass & Dirt layer
			if (y == height - 1)
				return VoxelTypes.Grass;
			if (y >= height - (dirtHeight + 1))
				return VoxelTypes.Dirt;

			// Everything else is stone with a random chance of a gem block
			return Random.Next(0, 50) == 1
				? VoxelTypes.Diamond
				: VoxelTypes.Stone;
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