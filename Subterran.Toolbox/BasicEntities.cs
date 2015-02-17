using Subterran.Rendering.Components;
using Subterran.Toolbox.Components;

namespace Subterran.Toolbox
{
	public static class BasicEntities
	{
		public static Entity CreateTestBlockEntity()
		{
			return new Entity
			{
				Components =
				{
					new MeshRendererComponent(),
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