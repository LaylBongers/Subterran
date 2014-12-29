﻿using OpenTK;
using Subterran;
using Subterran.Basic;
using Subterran.OpenTK;
using Subterran.OpenTK.Components;
using Subterran.Voxels;

namespace TropicalIsland
{
	internal static class TropicalIsland
	{
		public static BasicSubterranGame Create()
		{
			var game = new BasicSubterranGame("Tropical Island");
			game.World = new Entity
			{
				Children =
				{
					CreateCameraEntity(game.Input),
					CreateWorldMapEntity()
				}
			};
			
			return game;
		}

		private static Entity CreateWorldMapEntity()
		{
			var voxelMapComponent = new FixedSizeVoxelMapComponent(5, 5, 5);

			// Fill the bottom of the voxel map
			var voxels = voxelMapComponent.Voxels;
			VoxelMapGenerator.GenerateRandomIn(voxels);

			return new Entity
			{
				Components =
				{
					voxelMapComponent
				}
			};
		}

		private static Entity CreateCameraEntity(InputManager input)
		{
			return new Entity
			{
				Position = new Vector3(0, 2, 4),
				Rotation = new Vector3(-0.05f * StMath.Tau, 0, 0),
				Components =
				{
					new CameraComponent
					{
						VerticalFoV = 0.2f*StMath.Tau,
						ZNear = 0.1f,
						ZFar = 100f
					},
					new NoclipMovementComponent(input)
				}
			};
		}
	}
}