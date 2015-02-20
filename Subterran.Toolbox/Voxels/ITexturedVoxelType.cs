using OpenTK;

namespace Subterran.Toolbox.Voxels
{
	public struct TextureLocation
	{
		public Vector2 Start { get; set; }
		public Vector2 End { get; set; }

		public TextureLocation(Vector2 start, Vector2 end)
			:this()
		{
			Start = start;
			End = end;
		}
	}

	public interface ITexturedVoxelType
	{
		TextureLocation GetTexture(VoxelSide side);
	}
}