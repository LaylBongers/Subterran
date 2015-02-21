using System;
using OpenTK;
using Subterran.Toolbox.Materials;

namespace Subterran.Toolbox.Voxels
{
	public static class ColoredVoxelMesher
	{
		public static ColoredVertex[] GenerateCubes(ColoredVoxel[,,] voxels)
		{
			return VoxelMesher.GenerateCubes(voxels, VoxelMesherListCache.Colored, 
				(voxel, vertex) => new ColoredVertex
				{
					Position = vertex.Position,
					Color = voxel.Color
				},
				voxel => voxel.IsSolid);
		}

		public static TexturedVertex[] GenerateCubesWithTexture(ColoredVoxel[,,] voxels)
		{
			return VoxelMesher.GenerateCubes(voxels, VoxelMesherListCache.Textured, 
				(voxel, vertex) => new TexturedVertex
				{
					Position = vertex.Position,
					TexCoord = GetTexCoordForCorner(vertex.Corner)
				},
				voxel => voxel.IsSolid);
		}

		private static Vector2 GetTexCoordForCorner(VoxelSideCorner corner)
		{
			switch (corner)
			{
				case VoxelSideCorner.TopLeft:
					return new Vector2(0, 0);
				case VoxelSideCorner.TopRight:
					return new Vector2(1, 0);
				case VoxelSideCorner.BottomLeft:
					return new Vector2(0, 1);
				case VoxelSideCorner.BottomRight:
					return new Vector2(1, 1);
			}

			throw new InvalidOperationException("Unknown quad corner type!");
		}
	}
}