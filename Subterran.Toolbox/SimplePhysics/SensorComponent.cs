using System;
using System.Linq;

namespace Subterran.Toolbox.SimplePhysics
{
	public sealed class SensorComponent : EntityComponent, IInitializable
	{
		public string Name { get; set; }
		public CubeCollider Collider { get; set; }
		public bool IsTriggered { get; private set; }

		public void Initialize()
		{
			var world = Entity.Parent.RequireComponent<PhysicsWorldComponent>();
			world.Updated += WorldUpdated;
		}

		private void WorldUpdated(object sender, EventArgs e)
		{
			var sensorBoundingBox = BoundingBox.FromPositionAndCollider(Entity.Transform.Position, Collider);

			var fixedBoxes = PhysicsHelper.FindFixedBoundingBoxes(Entity.Parent);
			var smartBoxes = PhysicsHelper.FindSmartBoundingBoxes(Entity.Parent, sensorBoundingBox);
			var combinedBoxes = fixedBoxes.Concat(smartBoxes);

			IsTriggered = combinedBoxes.Any(fixedBoundingBox => PhysicsHelper.CheckCollision(sensorBoundingBox, fixedBoundingBox));
		}
	}
}