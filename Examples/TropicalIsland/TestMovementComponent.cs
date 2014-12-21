using System;
using Subterran;

namespace TropicalIsland
{
	internal class TestMovementComponent : EntityComponent
	{
		private double _accumulator;

		public override void Update(Entity entity, TimeSpan elapsed)
		{
			_accumulator += elapsed.TotalSeconds;
			entity.Transform.Position = new WorldPosition((float) Math.Sin(_accumulator), 0, 0);
		}
	}
}