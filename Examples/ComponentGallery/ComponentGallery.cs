using System.Drawing;
using System.Linq;
using OpenTK;
using Subterran;
using Subterran.Rendering.Components;
using Subterran.Toolbox;
using Subterran.Toolbox.Voxels;

namespace ComponentGallery
{
	internal static class ComponentGallery
	{
		public static BasicSubterranGame Create()
		{
			var game = new BasicSubterranGame {Window = {Title = "Component Gallery"}};

			var testMap = VoxelMapSerializer.Load("./Objects/testmap.voxelmap");
			var teapot = ModelLoader.Load("./Objects/teapot.st.obj");

			// Create a special teapot with a smaller teapot on it
			var teapotWithChild = CreateTeapotEntity(new Vector3(18, 1, -15), -0.4f, teapot);
			teapotWithChild.Children.Add(new Entity
			{
				Position = new Vector3(1, 0.8f, 0),
				Scale = new Vector3(0.2f),
				Components =
				{
					new MeshRendererComponent {Vertices = teapot},
					new SpinnerComponent {Speed = 0.4f}
				}
			});

			game.World = new Entity
			{
				Children =
				{
					CreateCameraEntity(game.Window),
					CreateTopDownCameraEntity(teapotWithChild.Children.First()),

					// Random generated voxel maps
					CreateWorldMapEntity(new Vector3(0, 0, 0), Vector3.Zero, 0.5f, VoxelMapGenerator.GenerateFlat(25, 25)),
					CreateWorldMapEntity(new Vector3(0, 0, -26), Vector3.Zero, 1f, VoxelMapGenerator.GenerateFlat(25, 25)),
					CreateWorldMapEntity(new Vector3(26, 0, 0), Vector3.Zero, 0.5f, VoxelMapGenerator.GenerateRandom(25, 25)),
					CreateWorldMapEntity(new Vector3(26, 0, -26), Vector3.Zero, 1f, VoxelMapGenerator.GenerateRandom(25, 25)),
					CreateWorldMapEntity(new Vector3(-2, 0, 0), Vector3.Zero, 0.5f, VoxelMapGenerator.GenerateFlat(1, 1)),
					CreateWorldMapEntity(new Vector3(-2, 0, -2), Vector3.Zero, 1f, VoxelMapGenerator.GenerateFlat(1, 1)),

					// Perlin random generated voxel map
					CreateWorldMapEntity(new Vector3(52, 0, -26), Vector3.Zero, 0.5f, VoxelMapGenerator.GeneratePerlin(52, 25, 77)),

					// Test loaded in voxel maps
					CreateWorldMapEntity(new Vector3(13.5f, 0, 0), Vector3.Zero, 0.5f, testMap),
					CreateWorldMapEntity(new Vector3(19.5f, 5, 0), new Vector3(StMath.Tau*0.25f, 0, 0), 0.5f, testMap),
					CreateWorldMapEntity(new Vector3(17.5f, 0, 9f), new Vector3(0, StMath.Tau*0.125f, 0), 0.5f, testMap),

					// Example loaded in teapot
					CreateTeapotEntity(new Vector3(7, 1, -10), 0.8f, teapot),
					teapotWithChild
				}
			};

			return game;
		}

		private static Entity CreateWorldMapEntity(Vector3 position, Vector3 rotation, float scale, ColoredVoxel[,,] voxels)
		{
			return new Entity
			{
				Position = position,
				Rotation = rotation,
				Scale = new Vector3(scale, scale, scale),
				Components =
				{
					new MeshRendererComponent(),
					new VoxelMapComponent<ColoredVoxel>
					{
						Voxels = voxels,
						MeshGenerator = ColoredVoxelMesher.MeshGenerator
					}
				}
			};
		}

		private static Entity CreateCameraEntity(Window window)
		{
			return new Entity
			{
				Position = new Vector3(25, 20, 40),
				Rotation = new Vector3(-0.05f*StMath.Tau, 0, 0),
				Components =
				{
					new CameraComponent(),
					new NoclipMovementComponent(window)
				}
			};
		}

		private static Entity CreateTopDownCameraEntity(Entity target)
		{
			return new Entity
			{
				Position = new Vector3(12, 8, -12),
				Rotation = new Vector3(-0.25f*StMath.Tau, 0, 0),
				Components =
				{
					new CameraComponent
					{
						Size = new Size(200, 150),
						Color = Color.Firebrick
					},
					new LookAtComponent(target)
				}
			};
		}

		private static Entity CreateTeapotEntity(Vector3 position, float speed, ColoredVertex[] vertices)
		{
			var entity = new Entity
			{
				Position = position,
				Scale = new Vector3(5),
				Components =
				{
					new MeshRendererComponent {Vertices = vertices},
					new SpinnerComponent {Speed = speed}
				}
			};

			return entity;
		}
	}
}