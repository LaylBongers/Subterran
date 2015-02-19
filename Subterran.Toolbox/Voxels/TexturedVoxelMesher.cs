using System;
using System.Collections.Generic;
using OpenTK;
using Subterran.Rendering.Materials;

namespace Subterran.Toolbox.Voxels
{
	public static class TexturedVoxelMesher
	{
		private static readonly List<TexturedVertex> WorkingList = new List<TexturedVertex>();

		public static TexturedVertex[] GenerateCubes(ColoredVoxel[,,] voxels)
		{
			return VoxelMesher.GenerateCubes(voxels, WorkingList,
				(voxel, vertex) => new TexturedVertex
				{
					Position = vertex.Position,
					TexCoord = GetTexCoordForCorner(vertex.Corner)
				},
				voxel => voxel.IsSolid);
		}

		private static Vector2 GetTexCoordForCorner(VoxelMesher.QuadCorner corner)
		{
			switch (corner)
			{
				case VoxelMesher.QuadCorner.TopLeft:
					return new Vector2(0, 0);
				case VoxelMesher.QuadCorner.TopRight:
					return new Vector2(1, 0);
				case VoxelMesher.QuadCorner.BottomLeft:
					return new Vector2(0, 1);
				case VoxelMesher.QuadCorner.BottomRight:
					return new Vector2(1, 1);
			}

			throw new InvalidOperationException("Unknown quad corner type!");
		}
	}
}