using System.Drawing;
using Subterran.Rendering.Components;
using Subterran.Toolbox.Voxels;

namespace Subterran.Toolbox
{
	public static class BasicEntities
	{
		public static Entity CreateTestBlockEntity()
		{
			return new Entity
			{
				Components =
				{
					new MeshRendererComponent(),
					new VoxelMapComponent<ColoredVoxel>
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
					}
				}
			};
		}
	}
}