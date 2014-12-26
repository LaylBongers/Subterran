using OpenTK;
using Subterran.Rendering;

namespace Subterran.Voxels
{
	public class FixedVoxelMapComponent : RenderEntityComponent
	{
		public bool[][][] Voxels { get; set; }

		public FixedVoxelMapComponent()
		{
		}

		public FixedVoxelMapComponent(int width, int height, int depth)
		{
			Voxels = new bool[width][][];
			for (var x = 0; x < Voxels.Length; x++)
			{
				Voxels[x] = new bool[height][];
				for (var y = 0; y < Voxels[x].Length; y++)
				{
					Voxels[x][y] = new bool[depth];
				}
			}
		}

		public override void Render(Renderer renderer, Matrix4 matrix)
		{
			for (var x = 0; x < Voxels.Length; x++)
			{
				for (var y = 0; y < Voxels[x].Length; y++)
				{
					for (var z = 0; z < Voxels[x][y].Length; z++)
					{
						if (!Voxels[x][y][z])
							continue;

						var voxelMatrix = Matrix4.CreateTranslation(x, y, z)*matrix;
						renderer.RenderMesh(ref voxelMatrix);
					}
				}
			}
		}
	}
}