using System;
using OpenTK;

namespace Subterran.Toolbox
{
	internal class LookAtComponent : EntityComponent, IUpdatable
	{
		private readonly Entity _target;

		public LookAtComponent(Entity target)
		{
			_target = target;
		}

		public void Update(TimeSpan elapsed)
		{
			var diff = _target.Position - Entity.Position;
			diff.NormalizeFast();

			var yaw = (float) Math.Atan2(diff.X, diff.Z);
			var pitch = (float) Math.Asin(diff.Y);

			Entity.Rotation = new Vector3(pitch, yaw + (0.5f*StMath.Tau), 0);
		}
	}
}