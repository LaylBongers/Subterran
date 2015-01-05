using OpenTK;
using Subterran;
using Subterran.OpenTK;
using Subterran.OpenTK.Components;
using Subterran.Toolbox;
using Subterran.Toolbox.Voxels;

namespace TropicalIsland
{
	internal static class TropicalIsland
	{
		public static BasicSubterranGame Create()
		{
			var game = new BasicSubterranGame("Tropical Island");

			var testMap = VoxelMapSerializer.Load("./Objects/test_map.voxelmap");
			var teapot = ModelLoader.Load("./Objects/teapot.st.obj");

			game.World = new Entity
			{
				Children =
				{
					CreateCameraEntity(game.Input),

					// Random generated voxel maps
					CreateWorldMapEntity(new Vector3(0, 0, 0), Vector3.Zero, 0.5f, VoxelMapGenerator.GenerateFlat(25, 25)),
					CreateWorldMapEntity(new Vector3(0, 0, -26), Vector3.Zero, 1f, VoxelMapGenerator.GenerateFlat(25, 25)),
					CreateWorldMapEntity(new Vector3(26, 0, 0), Vector3.Zero, 0.5f, VoxelMapGenerator.GenerateRandom(25, 25)),
					CreateWorldMapEntity(new Vector3(26, 0, -26), Vector3.Zero, 1f, VoxelMapGenerator.GenerateRandom(25, 25)),
					CreateWorldMapEntity(new Vector3(-2, 0, 0), Vector3.Zero, 0.5f, VoxelMapGenerator.GenerateFlat(1, 1)),
					CreateWorldMapEntity(new Vector3(-2, 0, -2), Vector3.Zero, 1f, VoxelMapGenerator.GenerateFlat(1, 1)),

					// Test loaded in voxel maps
					CreateWorldMapEntity(new Vector3(13.5f, 0, 0), Vector3.Zero, 0.5f, testMap),
					CreateWorldMapEntity(new Vector3(19.5f, 5, 0), new Vector3(StMath.Tau*0.25f, 0, 0), 0.5f, testMap),
					CreateWorldMapEntity(new Vector3(17.5f, 0, 9f), new Vector3(0, StMath.Tau*0.125f, 0), 0.5f, testMap),

					// Example loaded in teapot
					CreateModelEntity(new Vector3(7, 1, -10), 0.8f, teapot),
					CreateModelEntity(new Vector3(18, 1, -15), -0.4f, teapot)
				}
			};

			return game;
		}

		private static Entity CreateWorldMapEntity(Vector3 position, Vector3 rotation, float scale, Voxel[][][] voxels)
		{
			return new Entity
			{
				Position = position,
				Rotation = rotation,
				Scale = new Vector3(scale, scale, scale),
				Components =
				{
					new FixedSizeVoxelMapComponent {Voxels = voxels}
				}
			};
		}

		private static Entity CreateCameraEntity(InputManager input)
		{
			return new Entity
			{
				Position = new Vector3(25, 20, 50),
				Rotation = new Vector3(-0.05f*StMath.Tau, 0, 0),
				Components =
				{
					new CameraComponent
					{
						VerticalFoV = 0.2f*StMath.Tau,
						ZNear = 0.1f,
						ZFar = 200f
					},
					new NoclipMovementComponent(input)
				}
			};
		}

		private static Entity CreateModelEntity(Vector3 position, float speed, ColoredVertex[] vertices)
		{
			return new Entity
			{
				Position = position,
				Scale = new Vector3(5, 5, 5),
				Components =
				{
					new MeshRendererComponent {Vertices = vertices},
					new SpinnerComponent {Speed = speed}
				}
			};
		}
	}
}