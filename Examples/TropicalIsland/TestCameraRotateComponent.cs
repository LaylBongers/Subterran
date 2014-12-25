using System;
using Subterran;

namespace TropicalIsland
{
	public class TestCameraRotateComponent : EntityComponent
	{
		public override void Update(Entity entity, TimeSpan elapsed)
		{
			entity.Transform.Rotation += new WorldRotation(0, 0.01f, 0);
		}
	}
}