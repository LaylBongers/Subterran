using System;
using Subterran.Toolbox.Voxels;

namespace VoxelWorld.Generation
{
	public static class HouseGenerator
	{
		private const int HalfSize = 5;
		private const int Size = 10;

		public static void Generate(TexturedVoxel[,,] map, int x, int z)
		{
			var maxHeight = map.GetLength(1);

			// Find the height of the terrain at the target location
			var height = Math.Min(GetHeightOf(map, x, z), maxHeight - 7);

			// Place a platform at that area for reference
			GenerateEmptyAbove(map, x, height - 1, z); // We want at least 2 blocks under us
			GeneratePlatform(map, x, height, z);
			GenerateWalls(map,
				x - HalfSize, z - HalfSize, height + 1,
				Size, Size, 6);
			GenerateDoor(map,
				x - HalfSize + 1, z -HalfSize, height + 1,
				4, 5);
		}

		private static void GenerateDoor(TexturedVoxel[,,] map, int locX, int locZ, int locY, int width, int height)
		{
			var locEndX = locX + width;
			var locEndY = locY + height;

			for (var x = locX; x < locEndX; x++)
			{
				for (var y = locY; y < locEndY; y++)
				{
					// Edges except the bottom have to be wood
					if (x == locX || x == locEndX - 1 || y == locEndY - 1)
					{
						map[x, y, locZ].Type = VoxelTypes.Planks;
						continue;
					}

					// Anything else should be empty
					map[x, y, locZ].Type = null;
				}
			}
		}

		private static void GenerateWalls(
			TexturedVoxel[,,] map,
			int locX, int locZ, int locY,
			int sizeX, int sizeZ, int height)
		{
			var locEndX = locX + sizeX;
			var locEndZ = locZ + sizeZ;
			var locEndY = locY + height;

			for (var x = locX; x < locEndX; x++)
			{
				for (var y = locY; y < locEndY; y++)
				{
					for (var z = locZ; z < locEndZ; z++)
					{
						if (x != locX && x != locEndX - 1 && z != locZ && z != locEndZ - 1)
							continue;

						// Cobblestone on corner pillars
						if ((x == locX || x == locEndX - 1) && (z == locZ || z == locEndZ - 1))
						{
							map[x, y, z].Type = VoxelTypes.Cobblestone;
							continue;
						}

						// Wood on 1 from edge (except on the Y since that one doesn't have edges)
						if (x == locX + 1 || x == locEndX - 2 ||
						    y == locY || y == locEndY - 1 ||
						    z == locZ + 1 || z == locEndZ - 2)
						{
							map[x, y, z].Type = VoxelTypes.Planks;
							continue;
						}

						map[x, y, z].Type = VoxelTypes.Glass;
					}
				}
			}
		}

		private static int GetHeightOf(TexturedVoxel[,,] map, int x, int z)
		{
			for (var y = map.GetLength(1) - 1; y >= 0; y--)
			{
				if (map[x, y, z].Type != null)
					return y;
			}

			return 0;
		}

		private static void GenerateEmptyAbove(TexturedVoxel[,,] map, int locX, int locY, int locZ)
		{
			var maxHeight = map.GetLength(1);
			int startX = locX - HalfSize, endX = locX + HalfSize;
			int startZ = locZ - HalfSize, endZ = locZ + HalfSize;

			for (var x = startX; x < endX; x++)
			{
				for (var z = startZ; z < endZ; z++)
				{
					for (var y = locY; y < maxHeight; y++)
					{
						map[x, y, z].Type = null;
					}
				}
			}
		}

		private static void GeneratePlatform(TexturedVoxel[,,] map, int locX, int locY, int locZ)
		{
			int startX = locX - HalfSize, endX = locX + HalfSize;
			int startZ = locZ - HalfSize, endZ = locZ + HalfSize;

			for (var x = startX; x < endX; x++)
			{
				for (var z = startZ; z < endZ; z++)
				{
					for (var y = locY; y >= 0; y--)
					{
						// If there's a block at this location we've gone too far
						if (map[x, y, z].Type != null)
							break;

						// Place cobblestone at the current block
						map[x, y, z].Type = VoxelTypes.Cobblestone;
					}
				}
			}
		}
	}
}