using System;
using OpenTK;
using Subterran.Toolbox.Materials;

namespace Subterran.Toolbox.Voxels
{
	public static class TexturedVoxelMesher
	{
		// This compensates for floating point errors in texture filtering.
		private static readonly Vector2 ErrorCompensation = new Vector2(0.0001f, 0.0001f);

		public static TexturedVertex[] GenerateCubes(TexturedVoxel[, ,] voxels)
		{
			return VoxelMesher.GenerateCubes(voxels, VoxelMesherListCache.Textured,
				(voxel, vertex) => new TexturedVertex
				{
					Position = vertex.Position,
					TexCoord = GetTexCoordForCorner(vertex.Corner, voxel.Type.GetSide(vertex.Side))
				},
				voxel => voxel.Type != null,
				(current, bordering) =>
					// Blocks can be seen through air
					bordering.Type == null ||
					// Blocks can be seen through transparent blocks
					(bordering.Type.IsTransparent &&
						// Transparent blocks should not be visible through themselves (this prevents borders inside glass)
						!(current.Type.IsTransparent && current.Type == bordering.Type)));
		}

		private static Vector2 GetTexCoordForCorner(VoxelSideCorner corner, TextureLocation location)
		{
			switch (corner)
			{
				case VoxelSideCorner.TopLeft:
					return location.Start + ErrorCompensation;
				case VoxelSideCorner.TopRight:
					return new Vector2(
						location.End.X - ErrorCompensation.X,
						location.Start.Y + ErrorCompensation.Y);
				case VoxelSideCorner.BottomLeft:
					return new Vector2(
						location.Start.X + ErrorCompensation.X,
						location.End.Y - ErrorCompensation.Y);
				case VoxelSideCorner.BottomRight:
					return location.End - ErrorCompensation;
			}

			throw new InvalidOperationException("Unknown quad corner type!");
		}
	}
}