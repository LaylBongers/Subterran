﻿using System.Collections.Generic;
using System.Linq;
using OpenTK;

namespace Subterran.Toolbox.Voxels
{
	public static class VoxelMapMesher
	{
		public static List<ColoredVertex> GenerateMesh(Voxel[][][] voxels)
		{
			var vertices = new List<ColoredVertex>();

			for (var x = 0; x < voxels.Length; x++)
			{
				for (var y = 0; y < voxels[x].Length; y++)
				{
					for (var z = 0; z < voxels[x][y].Length; z++)
					{
						if (!voxels[x][y][z].IsSolid)
							continue;

						vertices.AddRange(GenerateVoxelMesh(
							x <= 0 || !voxels[x - 1][y][z].IsSolid,
							x >= voxels.Length - 1 || !voxels[x + 1][y][z].IsSolid,
							y <= 0 || !voxels[x][y - 1][z].IsSolid,
							y >= voxels[x].Length - 1 || !voxels[x][y + 1][z].IsSolid,
							z <= 0 || !voxels[x][y][z - 1].IsSolid,
							z >= voxels[x][y].Length - 1 || !voxels[x][y][z + 1].IsSolid)
							.Transform(Matrix4.CreateTranslation(x, y, z))
							.Select(v => new ColoredVertex
							{
								Position = v,
								Color = voxels[x][y][z].Color
							}));
					}
				}
			}

			return vertices;
		}

		public static Vector3[] GenerateVoxelMesh(bool left, bool right, bool bottom, bool top, bool back, bool front)
		{
			var square = new[]
			{
				new Vector3(0, 1, 0), // Left Top
				new Vector3(0, 0, 0), // Left Bottom
				new Vector3(1, 0, 0), // Right Bottom

				new Vector3(0, 1, 0), // Left Top
				new Vector3(1, 0, 0), // Right Bottom
				new Vector3(1, 1, 0) // Right Top
			};

			var vertices = new List<Vector3>();

			if (left)
			{
				var leftVoxelMatrix =
					Matrix4.CreateRotationY(-0.25f*StMath.Tau)*
					Matrix4.CreateTranslation(0, 0, 0);
				vertices.AddRange(square.Transform(leftVoxelMatrix));
			}

			if (right)
			{
				var rightVoxelMatrix =
					Matrix4.CreateRotationY(0.25f*StMath.Tau)*
					Matrix4.CreateTranslation(1, 0, 1);
				vertices.AddRange(square.Transform(rightVoxelMatrix));
			}

			if (top)
			{
				var topVoxelMatrix =
					Matrix4.CreateRotationX(-0.25f*StMath.Tau)*
					Matrix4.CreateTranslation(0, 1, 1);
				vertices.AddRange(square.Transform(topVoxelMatrix));
			}

			if (bottom)
			{
				var bottomVoxelMatrix =
					Matrix4.CreateRotationX(0.25f*StMath.Tau)*
					Matrix4.CreateTranslation(0, 0, 0);
				vertices.AddRange(square.Transform(bottomVoxelMatrix));
			}

			if (front)
			{
				var frontVoxelMatrix =
					Matrix4.CreateTranslation(0, 0, 1);
				vertices.AddRange(square.Transform(frontVoxelMatrix));
			}

			if (back)
			{
				var backVoxelMatrix =
					Matrix4.CreateRotationY(0.5f*StMath.Tau)*
					Matrix4.CreateTranslation(1, 0, 0);
				vertices.AddRange(square.Transform(backVoxelMatrix));
			}

			return vertices.ToArray();
		}
	}
}