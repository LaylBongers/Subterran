using Subterran.Toolbox.Voxels;

namespace VoxelWorld.Voxels
{
	public static class VoxelTypes
	{
		public static readonly ITexturedVoxelType Stone = new StoneVoxelType();
		public static readonly ITexturedVoxelType Cobblestone = new CobblestoneVoxelType();
		public static readonly ITexturedVoxelType Grass = new GrassVoxelType();
		public static readonly ITexturedVoxelType Dirt = new DirtVoxelType();
		public static readonly ITexturedVoxelType Diamond = new DiamondVoxelType();
		public static readonly ITexturedVoxelType Bedrock = new BedrockVoxelType();
		public static readonly ITexturedVoxelType Glass = new GlassVoxelType();
		public static readonly ITexturedVoxelType Planks = new PlanksVoxelType();
	}
}