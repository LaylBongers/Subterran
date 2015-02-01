using OpenTK;

namespace Subterran.Toolbox.SimplePhysics
{
	internal struct BoundingBox
	{
		public Vector3 Start { get; set; }
		public Vector3 End { get; set; }

		public static BoundingBox FromPositionAndCollider(Vector3 position, CubeCollider collider)
		{
			var boundingBox = new BoundingBox();

			boundingBox.Start = position + collider.Origin;
			boundingBox.End = boundingBox.Start + collider.Size;

			return boundingBox;
		}
	}
}