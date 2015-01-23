using System.Collections;
using System.Collections.Generic;

namespace Subterran.Toolbox
{
	/// <summary>
	///     A random access collection that automatically allocates deque pages when accessed.
	/// </summary>
	public class AutoDeque<T> : IList<T>
	{
		public IEnumerator<T> GetEnumerator()
		{
			throw new System.NotImplementedException();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public void Add(T item)
		{
			throw new System.NotImplementedException();
		}

		public void Clear()
		{
			throw new System.NotImplementedException();
		}

		public bool Contains(T item)
		{
			throw new System.NotImplementedException();
		}

		public void CopyTo(T[] array, int arrayIndex)
		{
			throw new System.NotImplementedException();
		}

		public bool Remove(T item)
		{
			throw new System.NotImplementedException();
		}

		public int Count { get; private set; }
		public bool IsReadOnly { get; private set; }
		public int IndexOf(T item)
		{
			throw new System.NotImplementedException();
		}

		public void Insert(int index, T item)
		{
			throw new System.NotImplementedException();
		}

		public void RemoveAt(int index)
		{
			throw new System.NotImplementedException();
		}

		public T this[int index]
		{
			get { throw new System.NotImplementedException(); }
			set { throw new System.NotImplementedException(); }
		}
	}
}