using System;

namespace Subterran.WorldState
{
	public abstract class EntityComponent
	{
		private bool _wasAssigned;

		public bool Initialized { get; private set; }

		protected Entity Entity { get; private set; }

		internal void UpdateEntityBinding(Entity entity)
		{
			if (_wasAssigned && entity != null)
			{
				throw new InvalidOperationException("Can not assign the same component to two different entities.");
			}

			Entity = entity;
			_wasAssigned = true;
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