using Subterran.Rendering;
using Subterran.Rendering.Materials;

namespace Subterran.Toolbox.Voxels
{
	public static class ColoredVoxelMesher
	{
		public static ColoredVertex[] GenerateCubes(ColoredVoxel[,,] voxels)
		{
			return VoxelMesher.GenerateCubes(voxels);
		}
	}
}