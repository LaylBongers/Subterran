using System.Drawing;
using OpenTK;
using Subterran;
using Subterran.Rendering.Components;
using Subterran.Toolbox;
using Subterran.Toolbox.Components;
using Subterran.Toolbox.SimplePhysics;
using Subterran.Toolbox.Voxels;

namespace Platformer
{
	internal static class Platformer
	{
		public static BasicSubterranGame Create()
		{
			var game = new BasicSubterranGame();

			var player = CreatePlayerEntity();

			game.World = new Entity
			{
				Components =
				{
					new PhysicsWorldComponent()
				},
				Children =
				{
					CreateScriptsEntity(game.Window),
					CreateTerrainEntity(),
					player,
					CreateCameraEntity(player)
				}
			};

			return game;
		}

		private static Entity CreateScriptsEntity(Window window)
		{
			return new Entity
			{
				Components =
				{
					new FpsCounterComponent(window) {Title = "Platformer Example"}
				}
			};
		}

		private static Entity CreateTerrainEntity()
		{
			var voxels = new ColoredVoxel[4,3,1];

			voxels[0, 0, 0].IsSolid = true;
			voxels[0, 0, 0].Color = StMath.NormalizeColor(Color.BlueViolet);
			voxels[1, 0, 0].IsSolid = true;
			voxels[2, 0, 0].IsSolid = true;
			voxels[2, 1, 0].IsSolid = true;
			voxels[2, 2, 0].IsSolid = true;
			voxels[3, 0, 0].IsSolid = true;

			return new Entity
			{
				Transform =
				{
				Scale = new Vector3(0.8f, 1, 1),
},
				Components =
				{
					new MeshRendererComponent(),
					new VoxelMapComponent<ColoredVoxel>
					{
						Voxels = voxels,
						MeshGenerator = ColoredVoxelMesher.GenerateCubes
					},
					new VoxelMapFixedbodyComponent<ColoredVoxel>
					{
						IsSolidChecker = v => v.IsSolid
					}
				}
			};
		}

		private static Entity CreatePlayerEntity()
		{
			return new Entity
			{
				Transform =
				{
					Position = new Vector3(0.5f, 5, 0.5f)
				},
				Components =
				{
					new MeshRendererComponent
					{
						Offset = new Vector3(-0.5f, 0, -0.5f)
					},
					BasicComponents.CreateTestBlockComponent(Color.ForestGreen),
					new RigidbodyComponent
					{
						Gravity = new Vector3(0, -14, 0),
						Collider = new CubeCollider
						{
							Origin = new Vector3(-0.5f, 0, -0.5f),
							Size = new Vector3(1, 1, 1)
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
					new PlayerMoveComponent
					{
						Speed = 5,
						JumpHeight = 10
					}
				}
			};
		}

		private static Entity CreateCameraEntity(Entity followTarget)
		{
			return new Entity
			{
				Components =
				{
					new CameraComponent(),
					new CameraFollowComponent
					{
						Target = followTarget,
						Distance = 10,
						HeightOffset = 2
					}
				}
			};
		}
	}
}