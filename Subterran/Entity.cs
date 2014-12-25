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
			Components = new Collection<IEntityComponent>();
			Behaviors = new Collection<EntityBehavior>();
		}

		public Transform Transform { get; set; }

		public Collection<Entity> Children { get; set; }

		public Collection<IEntityComponent> Components { get; set; }

		public Collection<EntityBehavior> Behaviors { get; set; }

		[Pure]
		public T GetComponent<T>()
			where T : IEntityComponent
		{
			return GetComponents<T>().FirstOrDefault();
		}

		[Pure]
		[LinqTunnel]
		public IEnumerable<T> GetComponents<T>()
			where T : IEntityComponent
		{
			return Components.OfType<T>();
		}

		[Pure]
		public T GetBehavior<T>()
			where T : EntityBehavior
		{
			return GetBehaviors<T>().FirstOrDefault();
		}

		[Pure]
		[LinqTunnel]
		public IEnumerable<T> GetBehaviors<T>()
			where T : EntityBehavior
		{
			return Behaviors.OfType<T>();
		}

		public void Update(TimeSpan elapsed)
		{
			foreach (var component in Behaviors)
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