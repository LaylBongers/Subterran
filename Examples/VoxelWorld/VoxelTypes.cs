using OpenTK;
using Subterran.Toolbox.Voxels;

namespace VoxelWorld
{
	public static class VoxelTypes
	{
		public static readonly TexturedVoxelType Stone = new TexturedVoxelType()
			.SetAllSides(new TextureLocation(new Vector2(0.0f, 0.25f), new Vector2(0.25f, 0.5f)));

		public static readonly TexturedVoxelType Cobblestone = new TexturedVoxelType()
			.SetAllSides(new TextureLocation(new Vector2(0.25f, 0.5f), new Vector2(0.5f, 0.75f)));

		public static readonly TexturedVoxelType Grass = new TexturedVoxelType()
			.SetVerticalSides(new TextureLocation(new Vector2(0.0f, 0.0f), new Vector2(0.25f, 0.25f)))
			.SetSide(VoxelSide.Top, new TextureLocation(new Vector2(0.25f, 0.0f), new Vector2(0.5f, 0.25f)))
			.SetSide(VoxelSide.Bottom, new TextureLocation(new Vector2(0.5f, 0.0f), new Vector2(0.75f, 0.25f)));

		public static readonly TexturedVoxelType Dirt = new TexturedVoxelType()
			.SetAllSides(new TextureLocation(new Vector2(0.5f, 0.0f), new Vector2(0.75f, 0.25f)));

		public static readonly TexturedVoxelType Diamond = new TexturedVoxelType()
			.SetAllSides(new TextureLocation(new Vector2(0.5f, 0.25f), new Vector2(0.75f, 0.5f)));

		public static readonly TexturedVoxelType Bedrock = new TexturedVoxelType()
			.SetAllSides(new TextureLocation(new Vector2(0.25f, 0.25f), new Vector2(0.5f, 0.5f)));

		public static readonly TexturedVoxelType Glass = new TexturedVoxelType()
			.SetAllSides(new TextureLocation(new Vector2(0.5f, 0.5f), new Vector2(0.75f, 0.75f)))
			.SetTransparent();

		public static readonly TexturedVoxelType Planks = new TexturedVoxelType()
			.SetAllSides(new TextureLocation(new Vector2(0.0f, 0.5f), new Vector2(0.25f, 0.75f)));
	}
}