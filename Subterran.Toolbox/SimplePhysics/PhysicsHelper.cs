namespace Subterran.Toolbox.SimplePhysics
{
	public static class PhysicsHelper
	{
		public static bool CheckCollision(BoundingBox rigidBox, BoundingBox fixedBox)
		{
			return rigidBox.Start.Y < fixedBox.End.Y && rigidBox.End.Y > fixedBox.Start.Y &&
			       rigidBox.Start.X < fixedBox.End.X && rigidBox.End.X > fixedBox.Start.X &&
			       rigidBox.Start.Z < fixedBox.End.Z && rigidBox.End.Z > fixedBox.Start.Z;
		}
	}
}