using System;
using OpenTK;

namespace Subterran.Toolbox.SimplePhysics
{
	public class RigidbodyComponent : EntityComponent, IInitializable
	{
		public Vector3 Gravity { get; set; }
		public Vector3 Velocity { get; set; }
		public CubeCollider Collider { get; set; }

		public void Initialize()
		{
			if(Collider == null)
				throw new InvalidOperationException("RigidbodyComponent requires the Collider property to be set.");
		}
	}
}