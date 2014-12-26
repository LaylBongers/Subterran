using Subterran;
using Subterran.Basic;
using Subterran.Rendering.Components;
using Subterran.Voxels;

namespace TropicalIsland
{
	internal static class TropicalIsland
	{
		public static BasicSubterranGame Create()
		{
			var game = new BasicSubterranGame("Tropical Island")
			{
				World =
				{
					Children =
					{
						CreateWorldMapEntity(),
						CreateCameraEntity()
					}
				}
			};

			return game;
		}

		private static Entity CreateWorldMapEntity()
		{
			var voxelMapComponent = new FixedVoxelMapComponent(5, 5, 5);

			var voxels = voxelMapComponent.Voxels;
			for (var x = 0; x < voxels.Length; x++)
			{
				for (var z = 0; z < voxels[x][0].Length; z++)
				{
					voxels[x][0][z] = true;
				}
			}

			return new Entity
			{
				Components =
				{
					voxelMapComponent
				}
			};
		}

		private static Entity CreateCameraEntity()
		{
			return new Entity
			{
				Transform =
				{
					Position = new WorldPosition(0, 2, 4),
					Rotation = new WorldRotation(-0.05f*StMath.Tau, 0, 0)
				},
				Components =
				{
					new CameraComponent
					{
						VerticalFoV = 0.2f*StMath.Tau,
						ZNear = 0.1f,
						ZFar = 100f
					}
				}
			};
		}
	}
}