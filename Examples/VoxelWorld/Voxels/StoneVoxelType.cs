using OpenTK;
using Subterran.Toolbox.Voxels;

namespace VoxelWorld.Voxels
{
	internal class StoneVoxelType : ITexturedVoxelType
	{
		public bool IsTransparent
		{
			get { return false; }
		}

		public TextureLocation GetTexture(VoxelSide side)
		{
			return new TextureLocation(new Vector2(0.0f, 0.25f), new Vector2(0.25f, 0.5f));
		}
	}
}