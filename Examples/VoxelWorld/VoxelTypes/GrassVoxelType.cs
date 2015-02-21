using System;
using OpenTK;
using Subterran.Toolbox.Voxels;

namespace VoxelWorld.VoxelTypes
{
	internal class GrassVoxelType : ITexturedVoxelType
	{
		public TextureLocation GetTexture(VoxelSide side)
		{
			switch (side)
			{
				case VoxelSide.Top:
					return new TextureLocation(new Vector2(0.25f, 0.0f), new Vector2(0.5f, 0.25f));
				case VoxelSide.Bottom:
					return new TextureLocation(new Vector2(0.5f, 0.0f), new Vector2(0.75f, 0.25f));

				case VoxelSide.East:
				case VoxelSide.West:
				case VoxelSide.North:
				case VoxelSide.South:
					return new TextureLocation(new Vector2(0.0f, 0.0f), new Vector2(0.25f, 0.25f));
			}

			throw new InvalidOperationException("Invalid voxel side!");
		}
	}
}