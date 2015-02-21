using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK;

namespace Subterran.Toolbox.Voxels
{
	public static class VoxelMesher
	{
		private static readonly VoxelVertex[] Square =
		{
			new VoxelVertex(new Vector3(0, 1, 0), VoxelSideCorner.TopLeft),
			new VoxelVertex(new Vector3(0, 0, 0), VoxelSideCorner.BottomLeft),
			new VoxelVertex(new Vector3(1, 0, 0), VoxelSideCorner.BottomRight),
			new VoxelVertex(new Vector3(0, 1, 0), VoxelSideCorner.TopLeft),
			new VoxelVertex(new Vector3(1, 0, 0), VoxelSideCorner.BottomRight),
			new VoxelVertex(new Vector3(1, 1, 0), VoxelSideCorner.TopRight)
		};

		private static VoxelVertex[][] _lookupTable;

		/// <summary>
		///     Generates a mesh from a 3D array of voxels.
		/// </summary>
		/// <remarks>Extremely performance critical, likely to cause GC problems.</remarks>
		/// <param name="voxels">A 3D array of voxels to be converted.</param>
		/// <param name="workingList">A list to use for working with vertex data to avoid re-growing a list.</param>
		/// <param name="vertexCreator">A function that creates a vertex based on the vertex and voxel data.</param>
		/// <param name="solidChecker">A function that returns true if the voxel is a solid block.</param>
		/// <param name="borderTransparentChecker">
		///     A function that returns true if the bordering voxel is transparent.
		///     (should include checking for air)
		/// </param>
		/// <returns>The array of vertices making up the mesh.</returns>
		public static TVertexType[] GenerateCubes<TVoxelType, TVertexType>(TVoxelType[,,] voxels,
			List<TVertexType> workingList,
			Func<TVoxelType, VoxelVertex, TVertexType> vertexCreator,
			Func<TVoxelType, bool> solidChecker, Func<TVoxelType, bool> borderTransparentChecker)
			where TVertexType : struct
		{
			var width = voxels.GetLength(0);
			var height = voxels.GetLength(1);
			var depth = voxels.GetLength(2);

			// Get a list to keep the vertices in until we know how many
			workingList.Clear();

			for (var x = 0; x < width; x++)
			{
				for (var y = 0; y < height; y++)
				{
					for (var z = 0; z < depth; z++)
					{
						if (!solidChecker(voxels[x, y, z]))
							continue;

						// Get the vectors for this voxel's mesh
						var vectors = LookupVoxelMesh(
							x <= 0 || borderTransparentChecker(voxels[x - 1, y, z]),
							x >= width - 1 || borderTransparentChecker(voxels[x + 1, y, z]),
							y <= 0 || borderTransparentChecker(voxels[x, y - 1, z]),
							y >= height - 1 || borderTransparentChecker(voxels[x, y + 1, z]),
							z <= 0 || borderTransparentChecker(voxels[x, y, z - 1]),
							z >= depth - 1 || borderTransparentChecker(voxels[x, y, z + 1]));

						// Offset them one by one and copy them over into the list
						var offset = new Vector3(x, y, z);
						for (var i = 0; i < vectors.Length; i++)
						{
							workingList.Add(vertexCreator(voxels[x, y, z], vectors[i].Offset(offset)));
						}
					}
				}
			}

			// Finally, trim anything we don't need from the array
			var verticesCount = workingList.Count;
			var finalArray = new TVertexType[verticesCount];
			for (var i = 0; i < verticesCount; i++)
			{
				finalArray[i] = workingList[i];
			}

			return finalArray;
		}

		private static VoxelVertex[] LookupVoxelMesh(
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

		private static VoxelVertex[][] GenerateLookupTable()
		{
			var table = new VoxelVertex[(int) Sides.Max][];
			for (Sides key = 0; key < Sides.Max; key++)
			{
				table[(int) key] = GenerateVoxelMesh(key).ToArray();
			}
			return table;
		}

		private static IEnumerable<VoxelVertex> GenerateVoxelMesh(Sides sides)
		{
			var vertices = new List<VoxelVertex>();

			// X axis
			if (sides.HasFlag(Sides.Left))
			{
				vertices.AddRange(GenerateVoxelSide(
					new Vector3(0, 0, 0),
					new Vector3(0, -0.25f*StMath.Tau, 0),
					VoxelSide.West));
			}

			if (sides.HasFlag(Sides.Right))
			{
				vertices.AddRange(GenerateVoxelSide(
					new Vector3(1, 0, 1),
					new Vector3(0, 0.25f*StMath.Tau, 0),
					VoxelSide.East));
			}

			// Y axis
			if (sides.HasFlag(Sides.Bottom))
			{
				vertices.AddRange(GenerateVoxelSide(
					new Vector3(0, 0, 0),
					new Vector3(0.25f*StMath.Tau, 0, 0),
					VoxelSide.Bottom));
			}

			if (sides.HasFlag(Sides.Top))
			{
				vertices.AddRange(GenerateVoxelSide(
					new Vector3(0, 1, 1),
					new Vector3(-0.25f*StMath.Tau, 0, 0),
					VoxelSide.Top));
			}

			// Z axis
			if (sides.HasFlag(Sides.Back))
			{
				vertices.AddRange(GenerateVoxelSide(
					new Vector3(1, 0, 0),
					new Vector3(0, 0.5f*StMath.Tau, 0),
					VoxelSide.South));
			}

			if (sides.HasFlag(Sides.Front))
			{
				vertices.AddRange(GenerateVoxelSide(
					new Vector3(0, 0, 1),
					new Vector3(0, 0, 0),
					VoxelSide.North));
			}

			return vertices;
		}

		private static IEnumerable<VoxelVertex> GenerateVoxelSide(Vector3 offset, Vector3 rotation, VoxelSide side)
		{
			var sideMatrix =
				Matrix4.CreateRotationX(rotation.X)*
				Matrix4.CreateRotationY(rotation.Y)*
				Matrix4.CreateRotationZ(rotation.Z)*
				Matrix4.CreateTranslation(offset);
			return Square.Select(v => v.Transform(sideMatrix, side));
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