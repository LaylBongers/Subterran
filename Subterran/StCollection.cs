using System;
using System.Collections.Specialized;
using System.Linq;

namespace Subterran
{
	public static class StCollection
	{
		public static void ExecuteForAdded<T>(NotifyCollectionChangedEventArgs args, Action<T> action)
		{
			if (args.NewItems == null)
				return;

			// Set this class as parent for the new added children
			foreach (var item in args.NewItems.Cast<T>())
			{
				action(item);
			}
		}

		public static void ExecuteForRemoved<T>(NotifyCollectionChangedEventArgs args, Action<T> action)
		{
			if (args.OldItems == null)
				return;

			// Unset this class as parent for the old removed children
			foreach (var item in args.OldItems.Cast<T>())
			{
				action(item);
			}
		}
	}
}
