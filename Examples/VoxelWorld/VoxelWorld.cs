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
					CreateVoxelWorldEntity()
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

		public static Entity CreateVoxelWorldEntity()
		{
			var voxels = MapGenerator.Generate(500, 25, 500);

			return new Entity
			{
				Scale = new Vector3(0.5f, 0.5f, 0.5f),
				Components =
				{
					new FixedSizeVoxelMapComponent {Voxels = voxels}
				}
			};
		}
	}
}