using System;
using System.Collections.Generic;
using OpenTK;
using Subterran.Rendering.Materials;

namespace Subterran.Toolbox.Voxels
{
	public static class TexturedVoxelMesher
	{
		public static TexturedVertex[] GenerateCubes(TexturedVoxel[, ,] voxels)
		{
			return VoxelMesher.GenerateCubes(voxels, new List<TexturedVertex>(), 
				(voxel, vertex) => new TexturedVertex
				{
					Position = vertex.Position,
					TexCoord = GetTexCoordForCorner(vertex.Corner, voxel.Type.GetTexture(vertex.Side))
				},
				voxel => voxel.Type != null);
		}

		private static Vector2 GetTexCoordForCorner(VoxelSideCorner corner, TextureLocation location)
		{
			switch (corner)
			{
				case VoxelSideCorner.TopLeft:
					return location.Start;
				case VoxelSideCorner.TopRight:
					return new Vector2(location.End.X, location.Start.Y);
				case VoxelSideCorner.BottomLeft:
					return new Vector2(location.Start.X, location.End.Y);
				case VoxelSideCorner.BottomRight:
					return location.End;
			}

			throw new InvalidOperationException("Unknown quad corner type!");
		}
	}
}