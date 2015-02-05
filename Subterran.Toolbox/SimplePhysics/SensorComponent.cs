using System.Linq;

namespace Subterran.Toolbox.SimplePhysics
{
	public class SensorComponent : EntityComponent
	{
		public string Name { get; set; }
		public CubeCollider Collider { get; set; }

		public bool CheckTriggered()
		{
			var sensorBoundingBox = BoundingBox.FromPositionAndCollider(Entity.Position, Collider);

			var fixedBoxes = PhysicsHelper.FindFixedBoundingBoxes(Entity.Parent);
			var smartBoxes = PhysicsHelper.FindSmartBoundingBoxes(Entity.Parent, sensorBoundingBox);
			var combinedBoxes = fixedBoxes.Concat(smartBoxes);

			return combinedBoxes.Any(fixedBoundingBox => PhysicsHelper.CheckCollision(sensorBoundingBox, fixedBoundingBox));
		}
	}
}