using Subterran.Rendering.Components;

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
	}
}