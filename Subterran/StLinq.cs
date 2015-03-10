using System.Collections.Generic;

namespace Subterran
{
	public static class StLinq
	{
		public static void AddTo<T>(this IEnumerable<T> source, ICollection<T> destination)
		{
			StContract.ArgumentNotNull(source, "source");
			StContract.ArgumentNotNull(destination, "destination");

			foreach (var item in source)
			{
				destination.Add(item);
			}
		}
	}
}