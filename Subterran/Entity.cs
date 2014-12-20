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
			Children = new Collection<Entity>();
			Components = new Collection<EntityComponent>();
		}

		public Collection<Entity> Children { get; set; }

		public Collection<EntityComponent> Components { get; set; }

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