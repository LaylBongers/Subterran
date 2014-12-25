using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using JetBrains.Annotations;

namespace Subterran
{
	public sealed class Entity
	{
		public Entity()
		{
			Transform = new Transform();
			Children = new Collection<Entity>();
			Components = new Collection<EntityComponent>();
		}

		public Transform Transform { get; set; }

		public Collection<Entity> Children { get; set; }

		public Collection<EntityComponent> Components { get; set; }

		[Pure]
		public T GetComponent<T>()
			where T : EntityComponent
		{
			return GetComponents<T>().FirstOrDefault();
		}

		[Pure]
		[LinqTunnel]
		public IEnumerable<T> GetComponents<T>()
			where T : EntityComponent
		{
			return Components.OfType<T>();
		}

		public void Update(TimeSpan elapsed)
		{
			foreach (var component in Components)
			{
				component.Update(this, elapsed);
			}

			foreach (var child in Children)
			{
				child.Update(elapsed);
			}
		}
	}
}