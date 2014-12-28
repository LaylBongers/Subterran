using System.Drawing;
using OpenTK;

namespace Subterran.OpenTK
{
	internal static class VectorExtensions
	{
		public static Vector3 ToVector3(this WorldPosition position)
		{
			return new Vector3(position.X, position.Y, position.Z);
		}

		public static Point ToPoint(this ScreenPosition position)
		{
			return new Point(position.X, position.Y);
		}

		public static Size ToSize(this ScreenSize size)
		{
			return new Size(size.X, size.Y);
		}
	}
}