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
			var game = new BasicSubterranGame("Voxel World");

			game.World = new Entity
			{
				Children =
				{
					CreateCameraEntity(game.Window),
					CreateVoxelWorldEntity(new Vector3(0, 0, 0)),
					CreateVoxelWorldEntity(new Vector3(-250, 0, 0)),
					CreateVoxelWorldEntity(new Vector3(0, 0, -250)),
					CreateVoxelWorldEntity(new Vector3(-250, 0, -250)),
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

		public static Entity CreateVoxelWorldEntity(Vector3 position)
		{
			var voxels = MapGenerator.Generate(500, 25, 500);

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