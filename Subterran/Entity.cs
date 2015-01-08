using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using OpenTK;

namespace Subterran
{
	public sealed class Entity
	{
		public Entity()
		{
			// Scale by default needs to be 1 because 0 will give invisible entities
			Scale = new Vector3(1, 1, 1);

			Children = new ObservableCollection<Entity>();
			Children.CollectionChanged += Children_CollectionChanged;

			Components = new ObservableCollection<EntityComponent>();
			Components.CollectionChanged += Components_CollectionChanged;
		}

		public Vector3 Position { get; set; }

		public Vector3 Rotation { get; set; }

		public Vector3 Scale { get; set; }

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
		{
			return GetComponents<T>().FirstOrDefault();
		}

		public IEnumerable<T> GetComponents<T>()
		{
			return Components.OfType<T>();
		}

		/// <summary>
		///     Calls the function func for each component matching T.
		///     Propagates method call to children.
		/// </summary>
		/// <typeparam name="T">The type of the components to call on.</typeparam>
		/// <param name="func">The function to call.</param>
		public void Call<T>(Action<T> func)
		{
			foreach (var component in GetComponents<T>())
			{
				func(component);
			}

			foreach (var child in Children)
			{
				child.Call(func);
			}
		}
	}
}