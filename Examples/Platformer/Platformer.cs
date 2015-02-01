using OpenTK;
using Subterran;
using Subterran.Rendering.Components;
using Subterran.Toolbox;
using Subterran.Toolbox.Components;
using Subterran.Toolbox.SimplePhysics;

namespace Platformer
{
	internal static class Platformer
	{
		public static BasicSubterranGame Create()
		{
			var game = new BasicSubterranGame();

			var player = CreatePlayerEntity();
			var camera = CreateCameraEntity(player);

			game.World = new Entity
			{
				Components =
				{
					new PhysicsWorldComponent()
				},
				Children =
				{
					CreateScriptsEntity(game.Window),
					CreateTestReferenceEntity(new Vector3(0, 0, 0)),
					CreateTestReferenceEntity(new Vector3(2, -3, 0)),
					CreateTestReferenceEntity(new Vector3(5, 0, 0)),
					player,
					camera
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

		private static Entity CreateTestReferenceEntity(Vector3 position)
		{
			return new Entity
			{
				Position = position,
				Components =
				{
					new MeshRendererComponent(),
					BasicComponents.CreateTestBlockComponent(),

					new FixedbodyComponent
					{
						Collider = new CubeCollider
						{
							Origin = new Vector3(0, 0, 0),
							Size = new Vector3(1, 1, 1)
						}
					}
				}
			};
		}

		private static Entity CreatePlayerEntity()
		{
			return new Entity
			{
				Position = new Vector3(0.5f, 5, 0.5f),
				Components =
				{
					new MeshRendererComponent
					{
						Offset = new Vector3(-0.5f, 0, -0.5f)
					},
					BasicComponents.CreateTestBlockComponent(),

					new RigidbodyComponent
					{
						Gravity = new Vector3(0, -15, 0),
						Collider = new CubeCollider
						{
							Origin = new Vector3(-0.5f, 0, -0.5f),
							Size = new Vector3(1, 1, 1)
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