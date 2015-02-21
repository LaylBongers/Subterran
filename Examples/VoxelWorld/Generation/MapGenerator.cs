using System;
using OpenTK;
using Subterran.Toolbox.Voxels;

namespace VoxelWorld.Generation
{
	internal static class MapGenerator
	{
		private const int Height = 50;
		private const int HeightOffset = 10;
		private static readonly Random Random = new Random();

		public static TexturedVoxel[,,] Generate(int width, int depth, Vector2 perlinOffset)
		{
			var map = new TexturedVoxel[width, Height, depth];

			// Generate the terrain
			TerrainGenerator.Generate(map, perlinOffset, HeightOffset);

			// Add a house
			var houseX = Random.Next(10, width - 10);
			var houseZ = Random.Next(10, depth - 10);
			HouseGenerator.Generate(map, houseX, houseZ);

			return map;
		}
	}
}