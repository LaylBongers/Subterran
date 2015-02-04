namespace Subterran.Toolbox.SimplePhysics
{
	public class SensorComponent : EntityComponent
	{
		public string Name { get; set; }
		public CubeCollider Collider { get; set; }

		public bool CheckTriggered()
		{
			var world = Entity.Parent;
			var sensorBoundingBox = BoundingBox.FromPositionAndCollider(Entity.Position, Collider);

			foreach (var entity in world.Children)
			{
				var component = entity.GetComponent<FixedbodyComponent>();

				// We can't use this entity if it is not a fixedbody
				if (component == null)
					continue;

				var fixedBoundingBox = BoundingBox.FromPositionAndCollider(entity.Position, component.Collider);

				if (PhysicsHelper.CheckCollision(sensorBoundingBox, fixedBoundingBox))
					return true;
			}

			return false;
		}
	}
}