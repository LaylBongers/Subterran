using System;
using System.Collections.Generic;
using System.Linq;

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

		public static IEnumerable<T> ConcatOne<T>(this IEnumerable<T> source, T value)
		{
			foreach (var srcValue in source)
			{
				yield return srcValue;
			}

			yield return value;
		}

		public static IEnumerable<T> ConcatOneIfNotDefault<T>(this IEnumerable<T> source, T value)
		{
			return value == null
				? source
				: source.ConcatOne(value);
		}
	}
}