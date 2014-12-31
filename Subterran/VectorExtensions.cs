using System.Collections.Generic;
using System.Linq;
using OpenTK;

namespace Subterran
{
	public static class VectorExtensions
	{
		public static IEnumerable<Vector3> Transform(this IEnumerable<Vector3> vectors, Matrix4 matrix)
		{
			return vectors.Select(v => Vector3.Transform(v, matrix));
		}
	}
}