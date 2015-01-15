using OpenTK;
using Subterran;
using Subterran.Rendering.Components;
using Subterran.Toolbox;
using Subterran.Toolbox.Voxels;

namespace VoxelWorld
{
	internal static class VoxelWorld
	{
		public static BasicSubterranGame Create()
		{
			var game = new BasicSubterranGame {Window = {Title = "Voxel World"}};

			game.World = new Entity
			{
				Children =
				{
					CreateCameraEntity(game.Window),
					CreateVoxelWorldEntity(new Vector3(0.5f, 0, 0.5f)),
					CreateVoxelWorldEntity(new Vector3(-200.5f, 0, 0.5f)),
					CreateVoxelWorldEntity(new Vector3(0.5f, 0, -200.5f)),
					CreateVoxelWorldEntity(new Vector3(-200.5f, 0, -200.5f)),
				}
			};

			return game;
		}

		private static Entity CreateCameraEntity(Window window)
		{
			return new Entity
			{
				Position = new Vector3(0, 50, 0),
				Rotation = new Vector3(0, -StMath.Tau*0.375f, 0),
				Components =
				{
					new CameraComponent(),
					new NoclipMovementComponent(window)
				}
			};
		}

		private static Entity CreateVoxelWorldEntity(Vector3 position)
		{
			var voxels = MapGenerator.Generate(400, 25, 400);

			return new Entity
			{
				Position = position,
				Scale = new Vector3(0.5f, 0.5f, 0.5f),
				Components =
				{
					new MeshRendererComponent(),
					new VoxelMapComponent {Voxels = voxels}
				}
			};
		}
	}
}