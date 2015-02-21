using OpenTK;
using Subterran;
using Subterran.Rendering;
using Subterran.Rendering.Components;
using Subterran.Toolbox;
using Subterran.Toolbox.Components;
using Subterran.Toolbox.Materials;
using Subterran.Toolbox.SimplePhysics;
using Subterran.Toolbox.Voxels;
using VoxelWorld.Generation;

namespace VoxelWorld
{
	internal static class VoxelWorld
	{
		public static BasicSubterranGame Create()
		{
			var game = new BasicSubterranGame();

			var fullbrightColor = BasicShaders.CreateFullbrightColor();
			var fullbrightTexture = BasicShaders.CreateFullbrightTexture();

			var playerMaterial = BasicMaterials.CreateFullbrightColor(fullbrightColor);
			var worldMaterial = BasicMaterials.CreateFullbrightTexture(fullbrightTexture, "./Graphics/Tilemap.png");
			var ominousMaterial = BasicMaterials.CreateFullbrightTexture(fullbrightTexture, "./Graphics/OminousCube.png");

			game.World = new Entity
			{
				Name = "World",
				Children =
				{
					CreateScriptsEntity(game),
					//BasicEntities.CreateNoclipCameraEntity(game.Window),
					CreatePlayerEntity(game.Window, playerMaterial),
					CreateOminousEntity(ominousMaterial),

					// Row #0
					//CreateVoxelWorldEntity(new Vector3(100, 0, 100)),
					//CreateVoxelWorldEntity(new Vector3(0, 0, 100)),
					//CreateVoxelWorldEntity(new Vector3(-100, 0, 100)),
					//CreateVoxelWorldEntity(new Vector3(-200, 0, 100)),

					// Row #1
					//CreateVoxelWorldEntity(new Vector3(100, 0, 0)),
					CreateVoxelWorldEntity(new Vector3(0, 0, 0), worldMaterial),
					CreateVoxelWorldEntity(new Vector3(-100, 0, 0), worldMaterial),
					//CreateVoxelWorldEntity(new Vector3(-200, 0, 0)),

					// Row #2
					//CreateVoxelWorldEntity(new Vector3(100, 0, -100)),
					CreateVoxelWorldEntity(new Vector3(0, 0, -100), worldMaterial),
					CreateVoxelWorldEntity(new Vector3(-100, 0, -100), worldMaterial)
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

		private static Entity CreateOminousEntity(Material<TexturedVertex> ominousMaterial)
		{
			var voxels = new ColoredVoxel[1, 1, 1];
			voxels[0, 0, 0].IsSolid = true;

			return new Entity
			{
				Name = "Ominous Cube",
				Children =
				{
					new Entity
					{
						Name = "Inner Actual Cube",
						Transform =
						{
							Position = new Vector3(0, 35, 0),
							Rotation = new Vector3(StMath.Tau*0.125f, 0, StMath.Tau * 0.125f),
							Scale = new Vector3(10)
						},
						Components =
						{
							new VoxelMapRendererComponent<ColoredVoxel, TexturedVertex>
							{
								Voxels = voxels,
								MeshGenerator = ColoredVoxelMesher.GenerateCubesWithTexture
							},
							new MeshRendererComponent<TexturedVertex>
							{
								Material = ominousMaterial,
								Offset = new Vector3(-0.5f, -0.5f, -0.5f)
							}
						}
					}
				},
				Components =
				{
					new SpinnerComponent()
				}
			};
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
					Position = new Vector3(0, 35, -20)
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

		private static Entity CreateVoxelWorldEntity(Vector3 position, Material<TexturedVertex> material)
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
					new MeshRendererComponent<TexturedVertex>
					{
						Material = material
					},
					new VoxelMapRendererComponent<TexturedVoxel, TexturedVertex>
					{
						Voxels = voxels,
						MeshGenerator = TexturedVoxelMesher.GenerateCubes
					},
					new VoxelMapFixedbodyComponent<TexturedVoxel>
					{
						Voxels = voxels,
						IsSolidChecker = v => v.Type != null
					}
				}
			};
		}
	}
}