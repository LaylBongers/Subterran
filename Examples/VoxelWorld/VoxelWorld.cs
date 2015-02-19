using OpenTK;
using Subterran;
using Subterran.Rendering;
using Subterran.Rendering.Components;
using Subterran.Rendering.Materials;
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

			var fullbrightColor = BasicMaterials.CreateFullbrightColor();
			//var fullbrightTexture = BasicMaterials.CreateFullbrightTexture("./Test.png");

			game.World = new Entity
			{
				Name = "World",
				Children =
				{
					CreateScriptsEntity(game),
					//BasicEntities.CreateNoclipCameraEntity(game.Window),
					CreatePlayerEntity(game.Window, fullbrightColor),

					// Row #0
					//CreateVoxelWorldEntity(new Vector3(100, 0, 100)),
					//CreateVoxelWorldEntity(new Vector3(0, 0, 100)),
					//CreateVoxelWorldEntity(new Vector3(-100, 0, 100)),
					//CreateVoxelWorldEntity(new Vector3(-200, 0, 100)),

					// Row #1
					//CreateVoxelWorldEntity(new Vector3(100, 0, 0)),
					CreateVoxelWorldEntity(new Vector3(0, 0, 0), fullbrightColor),
					CreateVoxelWorldEntity(new Vector3(-100, 0, 0), fullbrightColor),
					//CreateVoxelWorldEntity(new Vector3(-200, 0, 0)),

					// Row #2
					//CreateVoxelWorldEntity(new Vector3(100, 0, -100)),
					CreateVoxelWorldEntity(new Vector3(0, 0, -100), fullbrightColor),
					CreateVoxelWorldEntity(new Vector3(-100, 0, -100), fullbrightColor),
					//CreateVoxelWorldEntity(new Vector3(-200, 0, -100)),


					// Row #3
					//CreateVoxelWorldEntity(new Vector3(100, 0, -200)),
					//CreateVoxelWorldEntity(new Vector3(0, 0, -200)),
					//CreateVoxelWorldEntity(new Vector3(-100, 0, -200)),
					//CreateVoxelWorldEntity(new Vector3(-200, 0, -200))
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
				Name = "Scripts",
				Components =
				{
					new FpsCounterComponent(game.Window)
					{
						Title = "Voxel World"
					}
				}
			};
		}

		private static Entity CreatePlayerEntity(Window window, Material<ColoredVertex> material)
		{
			var collisionReferenceEntity = new Entity
			{
				Transform =
				{
					Scale = new Vector3(0.8f, 0.1f, 0.8f)
				},
				Components =
				{
					BasicComponents.CreateTestBlockComponent(),
					new MeshRendererComponent<ColoredVertex>
					{
						Material = material,
						Offset = new Vector3(-0.5f, 0, -0.5f)
					}
				}
			};

			var cameraEntity = new Entity
			{
				Transform =
				{
					Position = new Vector3(0, 1.5f, 0)
				},
				Components =
				{
					new CameraComponent()
				}
			};

			return new Entity
			{
				Name = "Player",
				Transform =
				{
					Position = new Vector3(0, 50, 0)
				},
				Children = {cameraEntity, collisionReferenceEntity},
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
							Origin = new Vector3(-0.4f, -0.01f, -0.4f),
							Size = new Vector3(0.8f, 0.01f, 0.8f)
						}
					},
					new PlayerMoveComponent(window)
					{
						Speed = 5,
						FastSpeed = 10,
						JumpHeight = 6,
						CameraEntity = cameraEntity
					},
					new PlayerAutoclimbComponent()
				}
			};
		}

		private static Entity CreateVoxelWorldEntity(Vector3 position, Material<ColoredVertex> material)
		{
			var voxels = MapGenerator.Generate(200, 200, position.Xz*2);

			return new Entity
			{
				Name = "Voxel World",
				Transform =
				{
					Position = position,
					Scale = new Vector3(0.5f)
				},
				Components =
				{
					new MeshRendererComponent<ColoredVertex>
					{
						Material = material
					},
					new VoxelMapComponent<ColoredVoxel, ColoredVertex>
					{
						Voxels = voxels,
						MeshGenerator = ColoredVoxelMesher.GenerateCubes
					},
					new VoxelMapFixedbodyComponent<ColoredVoxel, ColoredVertex>
					{
						IsSolidChecker = v => v.IsSolid
					}
				}
			};
		}
	}
}