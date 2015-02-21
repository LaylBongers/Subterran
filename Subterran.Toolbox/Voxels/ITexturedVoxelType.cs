using OpenTK;

namespace Subterran.Toolbox.Voxels
{
	public struct TextureLocation
	{
		public TextureLocation(Vector2 start, Vector2 end)
			: this()
		{
			Start = start;
			End = end;
		}

		public Vector2 Start { get; set; }
		public Vector2 End { get; set; }
	}

	public interface ITexturedVoxelType
	{
		bool IsTransparent { get; }
		TextureLocation GetTexture(VoxelSide side);
	}
}