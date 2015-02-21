using System;
using OpenTK;
using SharpNoise;
using SharpNoise.Modules;
using Subterran;
using Subterran.Toolbox.Voxels;
using VoxelWorld.VoxelTypes;

namespace VoxelWorld
{
	internal static class MapGenerator
	{
		private const int Height = 50;
		private const int HeightOffset = 10;
		private static readonly ITexturedVoxelType StoneType = new StoneVoxelType();
		private static readonly ITexturedVoxelType GrassType = new GrassVoxelType();
		private static readonly ITexturedVoxelType DirtType = new DirtVoxelType();
		private static readonly ITexturedVoxelType DiamondType = new DiamondVoxelType();
		private static readonly ITexturedVoxelType BedrockType = new BedrockVoxelType();

		public static TexturedVoxel[,,] Generate(int width, int depth, Vector2 perlinOffset)
		{
			var map = new TexturedVoxel[width, Height, depth];

			for (var x = 0; x < width; x++)
			{
				for (var z = 0; z < depth; z++)
				{
					GeneratePillar(map, x, z, perlinOffset);
				}
			}

			return map;
		}

		private static void GeneratePillar(TexturedVoxel[,,] map, int x, int z, Vector2 perlinOffset)
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

		private static TexturedVoxel GenerateBlock(int x, int y, int z, int pillarHeight, int dirtHeight, Vector2 perlinOffset)
		{
			var voxel = new TexturedVoxel();

			if (IsBedrock(y))
			{
				voxel.Type = BedrockType;
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
				return GrassType;
			if (y >= height - (dirtHeight + 1))
				return DirtType;

			// Everything else is stone with a random chance of a gem block
			return Random.Next(0, 50) == 1
				? DiamondType
				: StoneType;
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