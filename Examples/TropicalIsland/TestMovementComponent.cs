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
			entity.Transform.Position = new WorldPosition((float) Math.Sin(_accumulator), (float) Math.Sin(_accumulator*8)/20, 0);
			entity.Transform.Rotation = new WorldRotation(0, (float)_accumulator, 0);
		}
	}
}