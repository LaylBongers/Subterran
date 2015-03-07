using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Subterran
{
	[DebuggerDisplay("{DebuggerDisplay,nq}")]
	public sealed class Entity
	{
		private readonly Dictionary<Type, object> _getComponentsCache = new Dictionary<Type, object>();

		public Entity()
		{
			Name = "Entity";

			Transform.OwningEntity = this;

			Children.CollectionChanged += Children_CollectionChanged;
			Components.CollectionChanged += Components_CollectionChanged;
		}

		public string Name { get; set; }
		public Transform Transform { get; set; } = new Transform();
		public Entity Parent { get; private set; }
		public ObservableCollection<Entity> Children { get; } = new ObservableCollection<Entity>();
		public ObservableCollection<EntityComponent> Components { get; } = new ObservableCollection<EntityComponent>();

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		private string DebuggerDisplay => Name ?? "Anonymous Entity";

		private void Children_CollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
		{
			args.ExecuteForAdded<Entity>(i => i.Parent = this);
			args.ExecuteForRemoved<Entity>(i => i.Parent = null);
		}

		private void Components_CollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
		{
			// Invalidate the cache
			_getComponentsCache.Clear();

			args.ExecuteForAdded<EntityComponent>(i => i.UpdateEntityBinding(this));
			args.ExecuteForRemoved<EntityComponent>(i => i.UpdateEntityBinding(this));
		}

		public T GetOne<T>()
			where T : class
		{
			return GetMany<T>().FirstOrDefault();
		}

		public IEnumerable<T> GetMany<T>()
			where T : class
		{
			var type = typeof (T);

			// Try to retrieve from the cache
			object cachedComponents;
			if (_getComponentsCache.TryGetValue(type, out cachedComponents))
			{
				return (T[]) cachedComponents;
			}

			// We couldn't retrieve
			var components = Components.OfType<T>().ToArray();
			_getComponentsCache.Add(type, components);

			return components;
		}

		public T RequireOne<T>()
			where T : class
		{
			var value = GetOne<T>();

			if (value == null)
				throw new InvalidOperationException("This component requires a " + typeof (T).Name);

			return value;
		}

		public T RequireOne<T>(Func<T, bool> predicate)
			where T : class
		{
			var value = GetMany<T>().FirstOrDefault(predicate);

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
			if(func == null)
				throw new ArgumentNullException("func");

			foreach (var component in GetMany<T>())
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