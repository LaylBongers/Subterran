using System.Collections.Generic;
using Subterran.Rendering.Materials;

namespace Subterran.Toolbox.Voxels
{
	public static class ColoredVoxelMesher
	{
		private static readonly List<ColoredVertex> WorkingList = new List<ColoredVertex>();

		public static ColoredVertex[] GenerateCubes(ColoredVoxel[,,] voxels)
		{
			return VoxelMesher.GenerateCubes(voxels, WorkingList,
				(voxel, vertex) => new ColoredVertex
				{
					Position = vertex.Position,
					Color = voxel.Color
				},
				voxel => voxel.IsSolid);
		}
	}
}