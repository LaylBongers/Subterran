using System;
using OpenTK;

namespace Subterran.Toolbox.SimplePhysics
{
	public class RigidbodyComponent : EntityComponent, IUpdatable
	{
		public Vector3 Velocity { get; set; }

		public void Update(TimeSpan elapsed)
		{
			Entity.Position += elapsed.PerSecond(Velocity);
		}
	}
}