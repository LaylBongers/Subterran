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

		private void Children_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (e.NewItems != null)
			{
				// Set this class as parent for the new added children
				foreach (var item in e.NewItems.Cast<Entity>())
				{
					item.Parent = this;
				}
			}

			if (e.OldItems != null)
			{
				// Unset this class as parent for the old removed children
				foreach (var item in e.OldItems.Cast<Entity>())
				{
					item.Parent = null;
				}
			}
		}

		private void Components_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (e.NewItems != null)
			{
				// Set this class as entity for the new added components
				foreach (var item in e.NewItems.Cast<EntityComponent>())
				{
					item.UpdateEntityBinding(this);
				}
			}

			if (e.OldItems != null)
			{
				// Unset this class as entity for the old removed components
				foreach (var item in e.OldItems.Cast<EntityComponent>())
				{
					item.UpdateEntityBinding(null);
				}
			}
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