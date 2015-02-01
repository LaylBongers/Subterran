using System;

namespace Subterran.Toolbox.SimplePhysics
{
	public class FixedbodyComponent : EntityComponent, IInitializable
	{
		public CubeCollider Collider { get; set; }

		public void Initialize()
		{
			if (Collider == null)
				throw new InvalidOperationException("FixedbodyComponent requires the Collider property to be set.");
		}
	}
}