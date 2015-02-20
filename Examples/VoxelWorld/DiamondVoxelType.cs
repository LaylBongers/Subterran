using OpenTK;
using Subterran.Toolbox.Voxels;

namespace VoxelWorld
{
	internal class DiamondVoxelType : ITexturedVoxelType
	{
		public TextureLocation GetTexture(VoxelSide side)
		{
			return new TextureLocation(new Vector2(0.5f, 0.25f), new Vector2(0.75f, 0.5f));
		}
	}
}