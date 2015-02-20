using OpenTK;
using Subterran.Toolbox.Voxels;

namespace VoxelWorld
{
	internal class DirtVoxelType : ITexturedVoxelType
	{
		public TextureLocation GetTexture(VoxelSide side)
		{
			return new TextureLocation(new Vector2(0.5f, 0.0f), new Vector2(0.75f, 0.25f));
		}
	}
}