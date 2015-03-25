using OpenTK;
using Subterran;
using Subterran.Rendering;
using Subterran.Toolbox;
using Subterran.Toolbox.Materials;
using Subterran.Toolbox.SimplePhysics;
using Subterran.Toolbox.Voxels;
using Subterran.WorldState;
using VoxelWorld.Generation;

namespace VoxelWorld
{
	internal static class VoxelWorld
	{
		public static BasicSubterranGame Create()
		{
			var game = new BasicSubterranGame();

			var fullbrightTextureShader = BasicShaders.CreateFullbrightTexture();

			var worldMaterial = BasicMaterials.CreateFullbrightTexture(fullbrightTextureShader, "./Graphics/Tilemap.png");
			var ominousMaterial = BasicMaterials.CreateFullbrightTexture(fullbrightTextureShader, "./Graphics/OminousCube.png");
			var targetReferenceMaterial = BasicMaterials.CreateFullbrightTexture(fullbrightTextureShader, "./Graphics/TargetReference.png");

			var blockTargetReferenceEntity = CreateBlockTargetReferenceEntity(targetReferenceMaterial);

			game.World = new Entity
			{
				Name = "World",
				Children =
				{
					CreateScriptsEntity(game),
					blockTargetReferenceEntity,
					CreatePlayerEntity(game.Window, blockTargetReferenceEntity),
					CreateOminousEntity(ominousMaterial),
					CreateTexturedMeshEntity(fullbrightTextureShader),

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

		private static Entity CreateTexturedMeshEntity(Shader shader)
		{
			var capsule = Wavefront.LoadObj("./Graphics/Capsule.st.obj");
			var material = BasicMaterials.CreateFullbrightTexture(shader, capsule.Models[0].Material.Texture.FullName);
            var meshRenderer = new MeshRendererComponent<TexturedVertex>
			{
				Material = material
			};
			meshRenderer.SetMesh(capsule.Models[0].Mesh);

			return new Entity
			{
				Name = "Textured Mesh",
				Transform =
				{
					Position = new Vector3(20, 25, 0)
				},
				Components =
				{
					meshRenderer
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

		private static Entity CreateOminousEntity(Material<TexturedVertex> material)
		{
			var voxels = new ColoredVoxel[1, 1, 1];
			voxels[0, 0, 0].IsSolid = true;

			return new Entity
			{
				Name = "Ominous Cube",
				Transform =
				{
					Position = new Vector3(0, 35, 0)
				},
				Children =
				{
					new Entity
					{
						Name = "Inner Actual Cube",
						Transform =
						{
							Rotation = new Vector3(StMath.Tau*0.125f, 0, StMath.Tau*0.125f),
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
								Material = material,
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

		private static Entity CreateBlockTargetReferenceEntity(Material<TexturedVertex> material)
		{
			var voxels = new ColoredVoxel[1, 1, 1];
			voxels[0, 0, 0].IsSolid = true;

			return new Entity
			{
				Name = "Block Target Reference",
				Transform =
				{
					Scale = new Vector3(0.51f)
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
						Material = material,
						Offset = new Vector3(-0.5f, -0.5f, -0.5f)
					}
				}
			};
		}

		private static Entity CreatePlayerEntity(StandardWindowService window, Entity blockTargetReferenceEntity)
		{
			var cameraEntity = new Entity
			{
				Transform =
				{
					Position = new Vector3(0, 1.5f, 0)
				},
				Components =
				{
					new CameraComponent(),
					new AimPlaceBlockComponent
					{
						TargetReference = blockTargetReferenceEntity
					}
				}
			};

			return new Entity
			{
				Name = "Player",
				Transform =
				{
					Position = new Vector3(0, 35, -20)
				},
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