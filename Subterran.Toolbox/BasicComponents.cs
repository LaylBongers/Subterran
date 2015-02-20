using System.Drawing;
using Subterran.Toolbox.Materials;
using Subterran.Toolbox.Voxels;

namespace Subterran.Toolbox
{
	public static class BasicComponents
	{
		public static VoxelMapRendererComponent<ColoredVoxel, ColoredVertex> CreateTestBlockComponent(Color? color = null)
		{
			if (color == null)
			{
				color = Color.DarkRed;
			}

			return new VoxelMapRendererComponent<ColoredVoxel, ColoredVertex>
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