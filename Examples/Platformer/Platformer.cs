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

			var scripts = CreateScriptsEntity(game.Window);
			var player = CreatePlayerEntity();
			var camera = CreateCameraEntity(player);

			game.World = new Entity
			{
				Children =
				{
					scripts,
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

		private static Entity CreatePlayerEntity()
		{
			return new Entity
			{
				Components =
				{
					new RigidbodyComponent(),
					new PlayerMoveComponent()
				}
			};
		}

		private static Entity CreateCameraEntity(Entity followTarget)
		{
			return new Entity
			{
				Components =
				{
					new CameraComponent()
				}
			};
		}
	}
}