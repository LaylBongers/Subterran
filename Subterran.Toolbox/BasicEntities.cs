using Subterran.Rendering;
using Subterran.Toolbox.Materials;
using Subterran.WorldState;

namespace Subterran.Toolbox
{
	public static class BasicEntities
	{
		public static Entity CreateTestBlockEntity(Material<ColoredVertex> material)
		{
			return new Entity
			{
				Components =
				{
					new MeshRendererComponent<ColoredVertex>
					{
						Material = material
					},
					BasicComponents.CreateTestBlockComponent()
				}
			};
		}

		public static Entity CreateNoclipCameraEntity(StandardWindowService window)
		{
			return new Entity
			{
				Components =
				{
					new CameraComponent(),
					new NoclipMovementComponent(window)
				}
			};
		}
	}
}