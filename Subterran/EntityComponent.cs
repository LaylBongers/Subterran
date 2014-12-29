﻿using System;

namespace Subterran
{
	public abstract class EntityComponent
	{
		protected Entity Entity { get; private set; }

		internal void UpdateEntityBinding(Entity entity)
		{
			if (Entity != null && Entity != entity)
			{
				throw new InvalidOperationException("Can not assign the same component to two different entities.");
			}

			Entity = entity;
		}

		public virtual void Update(TimeSpan elapsed)
		{
			// This is just a stub so derived classes aren't required to implement it.
		}
	}
}