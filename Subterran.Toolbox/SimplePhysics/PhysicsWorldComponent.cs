using System;
using System.Linq;
using OpenTK;

namespace Subterran.Toolbox.SimplePhysics
{
	public class PhysicsWorldComponent : EntityComponent, IUpdatable
	{
		private TimeSpan _accumulator;

		public PhysicsWorldComponent()
		{
			// Default values
			Timestep = TimeSpan.FromSeconds(0.01);
		}

		public TimeSpan Timestep { get; set; }

		public void Update(TimeSpan elapsed)
		{
			// Optimization Note: Allocating tuples might cause GC issues, perhaps replace with structs?
			var rigidBodies = PhysicsHelper.FindRigidBodies(Entity);
			var fixedBoxes = PhysicsHelper.FindFixedBoundingBoxes(Entity);

			// We need to do a physics update once per time step
			_accumulator += elapsed;
			while (_accumulator > Timestep)
			{
				_accumulator -= Timestep;

				// Go through all of the rigid bodies 
				foreach (var rigidBody in rigidBodies)
				{
					// Put the original information in helper variables
					var collider = rigidBody.Item2.Collider;
					var velocity = rigidBody.Item2.Velocity;
					var originalPosition = rigidBody.Item1.Position;
					var originalBox = BoundingBox.FromPositionAndCollider(originalPosition, collider);

					// Add a bit of gravity
					velocity += elapsed.PerSecond(rigidBody.Item2.Gravity);

					// Find how much we'll move this tick
					var tickVelocity = Timestep.PerSecond(rigidBody.Item2.Velocity);

					// Find the expected position and bounding box
					var targetPosition = originalPosition + tickVelocity;
					var targetBox = BoundingBox.FromPositionAndCollider(targetPosition, collider);

					// Get all the domain-specific broadphase collected colliders
					var encompassing = CreateEncompassing(originalBox, targetBox);
					var smartBoxes = PhysicsHelper.FindSmartBoundingBoxes(Entity, encompassing);

					// Loop through all fixed bounding boxes
					foreach (var fixedBox in fixedBoxes.Concat(smartBoxes))
					{
						// TODO: Actually implement collision resolving
						if (PhysicsHelper.CheckCollision(targetBox, fixedBox))
						{
							tickVelocity = Vector3.Zero;
						}
					}

					// Submit the changes to the rigidbody
					rigidBody.Item1.Position = originalPosition + tickVelocity;
					rigidBody.Item2.Velocity = velocity;
				}
			}
		}

		private BoundingBox CreateEncompassing(BoundingBox left, BoundingBox right)
		{
			return new BoundingBox
			{
				Start = Vector3.ComponentMin(left.Start, right.Start),
				End = Vector3.ComponentMax(left.End, right.End)
			};
		}
	}
}