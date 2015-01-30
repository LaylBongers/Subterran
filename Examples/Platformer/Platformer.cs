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
				Children =
				{
					CreateScriptsEntity(game.Window),
					CreateTestReferenceEntity(),
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

		private static Entity CreateTestReferenceEntity()
		{
			return new Entity
			{
				Components =
				{
					new MeshRendererComponent(),
					BasicComponents.CreateTestBlockComponent()
				}
			};
		}

		private static Entity CreatePlayerEntity()
		{
			return new Entity
			{
				Components =
				{
					new MeshRendererComponent(),
					BasicComponents.CreateTestBlockComponent(),

					new RigidbodyComponent(),
					new PlayerMoveComponent
					{
						Speed = 5
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
					new CameraFollowComponent()
					{
						Target = followTarget,
						Distance = 8
					}
				}
			};
		}
	}
}