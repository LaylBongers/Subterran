using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK;

namespace Subterran.Toolbox.Voxels
{
	public static class ColoredVoxelMesher
	{
		private static Vector3[][] _lookupTable;

		public static ColoredVertex[] MeshGenerator(ColoredVoxel[,,] voxels)
		{
			var vertices = new List<ColoredVertex>();

			var width = voxels.GetLength(0);
			var height = voxels.GetLength(1);
			var depth = voxels.GetLength(2);

			for (var x = 0; x < width; x++)
			{
				for (var y = 0; y < height; y++)
				{
					for (var z = 0; z < depth; z++)
					{
						if (!voxels[x, y, z].IsSolid)
							continue;

						vertices.AddRange(LookupVoxelMesh(
							x <= 0 || !voxels[x - 1, y, z].IsSolid,
							x >= width - 1 || !voxels[x + 1, y, z].IsSolid,
							y <= 0 || !voxels[x, y - 1, z].IsSolid,
							y >= height - 1 || !voxels[x, y + 1, z].IsSolid,
							z <= 0 || !voxels[x, y, z - 1].IsSolid,
							z >= depth - 1 || !voxels[x, y, z + 1].IsSolid)
							.Transform(Matrix4.CreateTranslation(x, y, z))
							.Select(v => new ColoredVertex
							{
								Position = v,
								Color = voxels[x, y, z].Color
							}));
					}
				}
			}

			return vertices.ToArray();
		}

		private static IEnumerable<Vector3> LookupVoxelMesh(
			bool left, bool right,
			bool bottom, bool top,
			bool back, bool front)
		{
			if (_lookupTable == null)
			{
				_lookupTable = GenerateLookupTable();
			}

			// Generate a lookup key
			Sides sides = 0;
			if (left)
				sides = sides | Sides.Left;
			if (right)
				sides = sides | Sides.Right;
			if (bottom)
				sides = sides | Sides.Bottom;
			if (top)
				sides = sides | Sides.Top;
			if (back)
				sides = sides | Sides.Back;
			if (front)
				sides = sides | Sides.Front;

			// Now look up by the key in the table
			return _lookupTable[(int) sides];
		}

		private static Vector3[][] GenerateLookupTable()
		{
			var table = new Vector3[(int) Sides.Max][];
			for (Sides key = 0; key < Sides.Max; key++)
			{
				table[(int) key] = GenerateVoxelMesh(key).ToArray();
			}
			return table;
		}

		private static IEnumerable<Vector3> GenerateVoxelMesh(Sides sides)
		{
			// Benchmark: 23 seconds in VoxelWorld

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

			// X axis
			if (sides.HasFlag(Sides.Left))
			{
				var leftVoxelMatrix =
					Matrix4.CreateRotationY(-0.25f*StMath.Tau)*
					Matrix4.CreateTranslation(0, 0, 0);
				vertices.AddRange(square.Transform(leftVoxelMatrix));
			}

			if (sides.HasFlag(Sides.Right))
			{
				var rightVoxelMatrix =
					Matrix4.CreateRotationY(0.25f*StMath.Tau)*
					Matrix4.CreateTranslation(1, 0, 1);
				vertices.AddRange(square.Transform(rightVoxelMatrix));
			}

			// Y axis
			if (sides.HasFlag(Sides.Bottom))
			{
				var bottomVoxelMatrix =
					Matrix4.CreateRotationX(0.25f*StMath.Tau)*
					Matrix4.CreateTranslation(0, 0, 0);
				vertices.AddRange(square.Transform(bottomVoxelMatrix));
			}

			if (sides.HasFlag(Sides.Top))
			{
				var topVoxelMatrix =
					Matrix4.CreateRotationX(-0.25f*StMath.Tau)*
					Matrix4.CreateTranslation(0, 1, 1);
				vertices.AddRange(square.Transform(topVoxelMatrix));
			}

			// Z axis
			if (sides.HasFlag(Sides.Back))
			{
				var backVoxelMatrix =
					Matrix4.CreateRotationY(0.5f*StMath.Tau)*
					Matrix4.CreateTranslation(1, 0, 0);
				vertices.AddRange(square.Transform(backVoxelMatrix));
			}

			if (sides.HasFlag(Sides.Front))
			{
				var frontVoxelMatrix =
					Matrix4.CreateTranslation(0, 0, 1);
				vertices.AddRange(square.Transform(frontVoxelMatrix));
			}

			return vertices;
		}

		[Flags]
		private enum Sides
		{
			Left = 0x01,
			Right = 0x02,
			Bottom = 0x04,
			Top = 0x08,
			Back = 0x10,
			Front = 0x20,
			Max = 0x40
		}
	}
}