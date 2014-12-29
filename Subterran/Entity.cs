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
			Children = new Collection<Entity>();
			Components = new ObservableCollection<EntityComponent>();
			Components.CollectionChanged += Components_CollectionChanged;
		}

		public Vector3 Position { get; set; }

		public Vector3 Rotation { get; set; }

		public Collection<Entity> Children { get; private set; }

		public ObservableCollection<EntityComponent> Components { get; private set; }

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
			where T : EntityComponent
		{
			return GetComponents<T>().FirstOrDefault();
		}

		public IEnumerable<T> GetComponents<T>()
			where T : EntityComponent
		{
			return Components.OfType<T>();
		}

		public void Update(TimeSpan elapsed)
		{
			foreach (var component in Components)
			{
				component.Update(elapsed);
			}

			foreach (var child in Children)
			{
				child.Update(elapsed);
			}
		}
	}
}