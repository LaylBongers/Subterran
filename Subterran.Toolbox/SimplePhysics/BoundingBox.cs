using OpenTK;

namespace Subterran.Toolbox.SimplePhysics
{
	public struct BoundingBox
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

		public static  BoundingBox Encompassing(BoundingBox left, BoundingBox right)
		{
			return new BoundingBox
			{
				Start = Vector3.ComponentMin(left.Start, right.Start),
				End = Vector3.ComponentMax(left.End, right.End)
			};
		}

		public BoundingBox Translate(Vector3 value)
		{
			return new BoundingBox
			{
				Start = Start + value,
				End = End + value
			};
		}
	}
}