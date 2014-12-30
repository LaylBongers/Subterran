using OpenTK;
using OpenTK.Graphics.OpenGL;
using Subterran.OpenTK;

namespace Subterran.Voxels
{
	public class FixedSizeVoxelMapComponent : RenderEntityComponent
	{
		public FixedSizeVoxelMapComponent()
			: this(0, 0, 0)
		{
		}

		public FixedSizeVoxelMapComponent(int width, int height, int depth)
		{
			Voxels = StArray.CreateJagged<Voxel[][][]>(width, height, depth);
		}

		public Voxel[][][] Voxels { get; set; }

		public override void Render(Renderer renderer, Matrix4 matrix)
		{
			for (var x = 0; x < Voxels.Length; x++)
			{
				for (var y = 0; y < Voxels[x].Length; y++)
				{
					for (var z = 0; z < Voxels[x][y].Length; z++)
					{
						if (!Voxels[x][y][z].IsSolid)
							continue;

						// TODO: Change this to a proper mesh once RenderMesh takes one
						GL.Color3(Voxels[x][y][z].Color);

						var voxelMatrix =
							Matrix4.CreateTranslation(x, y, z)*
							Matrix4.CreateScale(0.5f)*
							matrix;
						renderer.RenderMesh(ref voxelMatrix);

						var leftVoxelMatrix =
							Matrix4.CreateRotationY(-0.25f*StMath.Tau)*
							Matrix4.CreateTranslation(-0.5f, 0, -0.5f)*
							voxelMatrix;
						renderer.RenderMesh(ref leftVoxelMatrix);

						var rightVoxelMatrix =
							Matrix4.CreateRotationY(0.25f*StMath.Tau)*
							Matrix4.CreateTranslation(0.5f, 0, -0.5f)*
							voxelMatrix;
						renderer.RenderMesh(ref rightVoxelMatrix);

						var backVoxelMatrix =
							Matrix4.CreateRotationY(0.5f*StMath.Tau)*
							Matrix4.CreateTranslation(0, 0, -1)*
							voxelMatrix;
						renderer.RenderMesh(ref backVoxelMatrix);

						var topVoxelMatrix =
							Matrix4.CreateRotationX(-0.25f*StMath.Tau)*
							Matrix4.CreateTranslation(0, 0.5f, -0.5f)*
							voxelMatrix;
						renderer.RenderMesh(ref topVoxelMatrix);
					}
				}
			}
		}
	}
}