using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace Subterran
{
	public sealed class Entity
	{

		public Entity()
		{
			Transform = new Transform();
			Transform.OwningEntity = this;

			Children = new ObservableCollection<Entity>();
			Children.CollectionChanged += Children_CollectionChanged;

			Components = new ObservableCollection<EntityComponent>();
			Components.CollectionChanged += Components_CollectionChanged;
		}

		public Transform Transform { get; set; }

		public Entity Parent { get; private set; }
		public ObservableCollection<Entity> Children { get; private set; }
		public ObservableCollection<EntityComponent> Components { get; private set; }

		private void Children_CollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
		{
			StCollection.ExecuteForAdded<Entity>(args, i => i.Parent = this);
			StCollection.ExecuteForRemoved<Entity>(args, i => i.Parent = null);
		}

		private void Components_CollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
		{
			StCollection.ExecuteForAdded<EntityComponent>(args, i => i.UpdateEntityBinding(this));
			StCollection.ExecuteForRemoved<EntityComponent>(args, i => i.UpdateEntityBinding(this));
		}

		public T GetComponent<T>()
			where T : class
		{
			return GetComponents<T>().FirstOrDefault();
		}

		public IEnumerable<T> GetComponents<T>()
			where T : class
		{
			for (var i = 0; i < Components.Count; i++)
			{
				var tComponent = Components[i] as T;
				if (tComponent != null)
					yield return tComponent;
			}

			//return Components.OfType<T>();
		}

		public T RequireComponent<T>()
			where T : class
		{
			var value = GetComponent<T>();

			if (value == null)
				throw new InvalidOperationException("This component requires a " + typeof (T).Name);

			return value;
		}

		public T RequireComponent<T>(Func<T, bool> predicate)
			where T : class
		{
			var value = GetComponents<T>().FirstOrDefault(predicate);

			if (value == null)
				throw new InvalidOperationException(
					"This component requires a " + typeof (T).Name +
					" matching given requirements.");

			return value;
		}

		/// <summary>
		///     Calls the function func for each component matching T.
		///     Propagates method call to children.
		/// </summary>
		/// <typeparam name="T">The type of the components to call on.</typeparam>
		/// <param name="func">The function to call.</param>
		public void ForEach<T>(Action<T> func)
			where T : class
		{
			foreach (var component in GetComponents<T>())
			{
				func(component);
			}

			foreach (var child in Children)
			{
				child.ForEach(func);
			}
		}
	}
}