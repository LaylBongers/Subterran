using OpenTK;
using Subterran;
using Subterran.Rendering.Components;
using Subterran.Toolbox;
using Subterran.Toolbox.Components;
using Subterran.Toolbox.SimplePhysics;
using Subterran.Toolbox.Voxels;

namespace VoxelWorld
{
	internal static class VoxelWorld
	{
		public static BasicSubterranGame Create()
		{
			var game = new BasicSubterranGame();

			game.World = new Entity
			{
				Children =
				{
					CreateScriptsEntity(game),
					CreatePlayerEntity(game.Window),
					CreateVoxelWorldEntity(new Vector3(0, 0, 0)),
					CreateVoxelWorldEntity(new Vector3(-100, 0, 0)),
					CreateVoxelWorldEntity(new Vector3(0, 0, -100)),
					CreateVoxelWorldEntity(new Vector3(-100, 0, -100))
				},
				Components =
				{
					new PhysicsWorldComponent()
				}
			};

			return game;
		}

		private static Entity CreateScriptsEntity(BasicSubterranGame game)
		{
			// Misc useful scripts that just need to be added to the scene

			return new Entity
			{
				Components =
				{
					new FpsCounterComponent(game.Window)
					{
						Title = "Voxel World"
					}
				}
			};
		}

		private static Entity CreatePlayerEntity(Window window)
		{
			var cameraEntity = new Entity
			{
				Position = new Vector3(0, 1.5f, 0),
				Components =
				{
					new CameraComponent()
				}
			};

			return new Entity
			{
				Position = new Vector3(0, 50, 0),
				Children = {cameraEntity},
				Components =
				{
					new RigidbodyComponent
					{
						Gravity = new Vector3(0, -14, 0),
						Collider = new CubeCollider
						{
							Origin = new Vector3(-0.4f, 0, -0.4f),
							Size = new Vector3(0.8f, 1.8f, 0.8f)
						}
					},
					new SensorComponent
					{
						Name = "JumpSensor",
						Collider = new CubeCollider
						{
							Origin = new Vector3(-0.5f, -0.01f, -0.5f),
							Size = new Vector3(1, 0.01f, 1)
						}
					},
					new PlayerMoveComponent(window)
					{
						Speed = 5,
						JumpHeight = 5,
						CameraEntity = cameraEntity
					}
				}
			};
		}

		private static Entity CreateVoxelWorldEntity(Vector3 position)
		{
			var voxels = MapGenerator.Generate(200, 200, position.Xz*2);

			return new Entity
			{
				Position = position,
				Scale = new Vector3(0.5f),
				Components =
				{
					new MeshRendererComponent(),
					new VoxelMapComponent<ColoredVoxel>
					{
						Voxels = voxels,
						MeshGenerator = ColoredVoxelMesher.MeshGenerator
					},
					new VoxelMapFixedbodyComponent<ColoredVoxel>
					{
						IsSolidChecker = v => v.IsSolid
					}
				}
			};
		}
	}
}