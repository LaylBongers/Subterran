using System;

namespace Subterran
{
	public static class StArray
	{
		public static T CreateJagged<T>(params int[] lengths)
		{
			return (T) InitializeJagged(typeof (T).GetElementType(), 0, lengths);
		}

		private static object InitializeJagged(Type type, int index, int[] lengths)
		{
			var array = Array.CreateInstance(type, lengths[index]);

			// Check if we've got child arrays
			var elementType = type.GetElementType();
			if (elementType == null)
				return array;

			// We do so we need to recursive call ourself to initialize those as well
			for (var i = 0; i < lengths[index]; i++)
			{
				array.SetValue(InitializeJagged(elementType, index + 1, lengths), i);
			}

			return array;
		}
	}
}