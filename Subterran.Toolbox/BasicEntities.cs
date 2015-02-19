using Subterran.Rendering;
using Subterran.Rendering.Components;
using Subterran.Rendering.Materials;
using Subterran.Toolbox.Components;

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

		public static Entity CreateNoclipCameraEntity(Window window)
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