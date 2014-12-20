using OpenTK;

namespace Subterran.Rendering
{
	internal static class WorldVectorsExtensions
	{
		public static Vector3 ToVector3(this WorldPosition position)
		{
			return new Vector3(position.X, position.Y, position.Z);
		}
	}
}