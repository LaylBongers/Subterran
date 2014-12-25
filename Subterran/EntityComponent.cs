using System;

namespace Subterran
{
	public abstract class EntityComponent
	{
		public virtual void Update(Entity entity, TimeSpan elapsed)
		{
		}
	}
}