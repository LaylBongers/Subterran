using System;

namespace Subterran
{
	public abstract class EntityComponent
	{
		public virtual void Update(Entity entity, TimeSpan elapsed)
		{
			// This is just a stub so derived classes aren't required to implement it.
		}
	}
}