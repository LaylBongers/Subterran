using System.Collections.Generic;
using Subterran.Toolbox.Materials;

namespace Subterran.Toolbox.Voxels
{
	/// <summary>
	///     Provides stored lists for VoxelMesher to use to avoid re-allocating data.
	/// </summary>
	public static class VoxelMesherListCache
	{
		static VoxelMesherListCache()
		{
			Textured = new List<TexturedVertex>();
			Colored = new List<ColoredVertex>();
		}

		public static List<TexturedVertex> Textured { get; private set; }
		public static List<ColoredVertex> Colored { get; private set; }
	}
}