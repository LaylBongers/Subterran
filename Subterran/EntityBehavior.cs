using System;

namespace Subterran
{
	public abstract class EntityBehavior
	{
		public virtual void Update(Entity entity, TimeSpan elapsed)
		{
		}
	}
}