using System;
using Subterran;
using Subterran.Basic;
using Subterran.Rendering.Components;

namespace TropicalIsland
{
	internal sealed class TropicalIsland : BasicSubterranGame
	{
		private readonly Entity _testEntity;
		private double _accumulator;

		public TropicalIsland()
			: base("Tropical Island")
		{
			_testEntity = new Entity
			{
				Components =
				{
					new TestRenderComponent()
				}
			};
			World.Children.Add(_testEntity);
		}

		protected override void Dispose(bool managed)
		{
			if (managed)
			{
				// Dispose stuff here
			}

			base.Dispose(managed);
		}

		protected override void Update(TimeSpan elapsed)
		{
			base.Update(elapsed);

			_accumulator += elapsed.TotalSeconds;
			_testEntity.Transform.Position = new WorldPosition((float) Math.Sin(_accumulator), 0, 0);
		}
	}
}