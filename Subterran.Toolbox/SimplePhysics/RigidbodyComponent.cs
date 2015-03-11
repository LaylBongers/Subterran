using System;
using OpenTK;
using Subterran.WorldState;

namespace Subterran.Toolbox.SimplePhysics
{
	public class RigidbodyComponent : EntityComponent, IInitializable
	{
		public Vector3 Gravity { get; set; }
		public Vector3 Velocity { get; set; }
		public CubeCollider Collider { get; set; }
		public bool Enabled { get; set; }

		public RigidbodyComponent()
		{
			Enabled = true;
		}

		public void Initialize()
		{
			if(Collider == null)
				throw new InvalidOperationException("RigidbodyComponent requires the Collider property to be set.");
		}
	}
}