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
		{
			return GetComponents<T>().FirstOrDefault();
		}

		[LinqTunnel]
		public IEnumerable<T> GetComponents<T>()
		{
			return Components.OfType<T>();
		}
	}
}