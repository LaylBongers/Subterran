using System;
using Subterran;
using Subterran.Basic;
using Subterran.Rendering.Components;

namespace TropicalIsland
{
	internal static class TropicalIsland
	{
		public static BasicSubterranGame Create()
		{
			var game = new BasicSubterranGame("Tropical Island")
			{
				World = new Entity
				{
					Children =
					{
						CreateTestEntity(),
						CreateTestCameraEntity(new WorldPosition(0, 0, 2))
					}
				}
			};

			return game;
		}

		private static Entity CreateTestEntity()
		{
			return new Entity
			{
				Behaviors =
				{
					new TestRenderBehavior(),
					new TestMovementBehavior()
				},
				Children =
				{
					// This is the back face of this test component
					new Entity
					{
						Transform =
						{
							Rotation = new WorldRotation(0, (float) Math.PI, 0)
						},
						Behaviors =
						{
							new TestRenderBehavior()
						}
					}
				}
			};
		}

		private static Entity CreateTestCameraEntity(WorldPosition position)
		{
			return new Entity
			{
				Transform =
				{
					Position = position
				},
				Components =
				{
					new CameraComponent
					{
						VerticalFoV = 0.2f*StMath.Tau,
						ZNear = 0.1f,
						ZFar = 100f
					}
				},
				Behaviors =
				{
					new TestCameraRotateBehavior()
				}
			};
		}
	}
}