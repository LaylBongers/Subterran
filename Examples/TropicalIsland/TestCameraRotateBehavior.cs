using System;
using Subterran;

namespace TropicalIsland
{
	public class TestCameraRotateBehavior : EntityBehavior
	{
		private double _accumulator;

		public override void Update(Entity entity, TimeSpan elapsed)
		{
			_accumulator += elapsed.TotalSeconds;
			entity.Transform.Rotation = new WorldRotation((float)Math.Sin(_accumulator) / 5, 0, 0);
		}
	}
}