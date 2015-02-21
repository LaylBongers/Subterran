using OpenTK;
using Subterran.Toolbox.Voxels;

namespace VoxelWorld.Voxels
{
	internal class GlassVoxelType : ITexturedVoxelType
	{
		public bool IsTransparent
		{
			get { return true; }
		}

		public TextureLocation GetTexture(VoxelSide side)
		{
			return new TextureLocation(new Vector2(0.5f, 0.5f), new Vector2(0.75f, 0.75f));
		}
	}
}