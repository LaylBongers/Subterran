using System.Drawing;
using Subterran.Toolbox.Voxels;

namespace Subterran.Toolbox
{
	public static class BasicComponents
	{
		public static EntityComponent CreateTestBlockComponent()
		{
			return new VoxelMapComponent<ColoredVoxel>
			{
				MeshGenerator = ColoredVoxelMesher.MeshGenerator,
				Voxels = new[,,]
				{
					{
						{
							new ColoredVoxel
							{
								IsSolid = true,
								Color = StMath.NormalizeColor(Color.DarkRed)
							}
						}
					}
				}
			};
		}
	}
}