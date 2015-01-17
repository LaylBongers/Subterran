using System;

namespace Subterran
{
	public abstract class EntityComponent
	{
		public bool Initialized { get; private set; }

		protected Entity Entity { get; private set; }

		internal void UpdateEntityBinding(Entity entity)
		{
			if (Entity != null)
			{
				throw new InvalidOperationException("Can not assign the same component to two different entities.");
			}

			Entity = entity;
		}

		public void CheckInitialize()
		{
			if (Initialized)
				return;

			Initialized = true;

			var updatableThis = this as IInitializable;
			if (updatableThis != null)
				updatableThis.Initialize();
		}
	}
}