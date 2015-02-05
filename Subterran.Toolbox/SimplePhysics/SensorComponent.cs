using System.Linq;

namespace Subterran.Toolbox.SimplePhysics
{
	public class SensorComponent : EntityComponent
	{
		public string Name { get; set; }
		public CubeCollider Collider { get; set; }

		public bool CheckTriggered()
		{
			var fixedBodies = PhysicsHelper.FindFixedBoundingBoxes(Entity.Parent);
			var sensorBoundingBox = BoundingBox.FromPositionAndCollider(Entity.Position, Collider);

			return fixedBodies.Any(fixedBoundingBox => PhysicsHelper.CheckCollision(sensorBoundingBox, fixedBoundingBox));
		}
	}
}