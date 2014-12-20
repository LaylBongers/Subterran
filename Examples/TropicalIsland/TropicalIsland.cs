using Subterran;
using Subterran.Basic;
using Subterran.Rendering.Components;

namespace TropicalIsland
{
	internal sealed class TropicalIsland : BasicSubterranGame
	{
		public TropicalIsland()
			: base("Tropical Island")
		{
			World.Children.Add(new Entity
			{
				Components =
				{
					new TestRenderComponent()
				}
			});
		}

		protected override void Dispose(bool managed)
		{
			if (managed)
			{
				// Dispose stuff here
			}

			base.Dispose(managed);
		}
	}
}