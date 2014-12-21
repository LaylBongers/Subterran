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
						CreateTestEntity()
					}
				}
			};

			return game;
		}

		private static Entity CreateTestEntity()
		{
			return new Entity
			{
				Components =
				{
					new TestRenderComponent(),
					new TestMovementComponent()
				}
			};
		}
	}
}