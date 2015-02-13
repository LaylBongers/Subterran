using System.Drawing;
using Subterran.Toolbox.Voxels;

namespace Subterran.Toolbox
{
	public static class BasicComponents
	{
		public static VoxelMapComponent<ColoredVoxel> CreateTestBlockComponent(Color? color = null)
		{
			if (color == null)
			{
				color = Color.DarkRed;
			}

			return new VoxelMapComponent<ColoredVoxel>
			{
				MeshGenerator = ColoredVoxelMesher.GenerateCubes,
				Voxels = new[,,]
				{
					{
						{
							new ColoredVoxel
							{
								IsSolid = true,
								Color = StMath.NormalizeColor(color.Value)
							}
						}
					}
				}
			};
		}
	}
}