using System;
using System.Collections.Generic;
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
					// Add a bit of gravity
					rigidBody.Item2.Velocity += elapsed.PerSecond(rigidBody.Item2.Gravity);

					MoveOnAxis(elapsed, rigidBody, fixedBoxes, v => v.X, (v, x) => v + new Vector3(x, 0, 0));
					MoveOnAxis(elapsed, rigidBody, fixedBoxes, v => v.Y, (v, y) => v + new Vector3(0, y, 0));
					MoveOnAxis(elapsed, rigidBody, fixedBoxes, v => v.Z, (v, z) => v + new Vector3(0, 0, z));
				}
			}
		}

		private void MoveOnAxis(TimeSpan elapsed,
			Tuple<Entity, RigidbodyComponent> rigidBody, IEnumerable<BoundingBox> fixedBoxes,
			Func<Vector3, float> axisGetter, Func<Vector3, float, Vector3> axisAdder)
		{
			// Get a bounding box for the new location
			var velocity = rigidBody.Item2.Velocity;
			var tickAxisVelocity = axisGetter(elapsed.PerSecond(velocity));
			var rigidBox = BoundingBox.FromPositionAndCollider(
				axisAdder(rigidBody.Item1.Position, tickAxisVelocity),
				rigidBody.Item2.Collider);

			// Get the smart collected bounding boxes we also need to check
			var smartBoxes = PhysicsHelper.FindSmartBoundingBoxes(Entity, rigidBox).ToList();

			// Check if any of the fixed boxes collide with our new position
			foreach (var fixedBox in fixedBoxes.Concat(smartBoxes))
			{
				// If we don't collide we're immediately done here
				if (!PhysicsHelper.CheckCollision(rigidBox, fixedBox))
					continue;

				// We do collide, so update our velocity for the collision
				tickAxisVelocity = ResolveAxisCollision(tickAxisVelocity, rigidBox, fixedBox, axisGetter);

				// Create a new bounding box based on the updated velocity
				rigidBox = BoundingBox.FromPositionAndCollider(
					axisAdder(rigidBody.Item1.Position, tickAxisVelocity),
					rigidBody.Item2.Collider);

				// Reset the velocity so we don't keep moving in the direction we just collided with
				velocity = axisAdder(velocity, -axisGetter(velocity));
			}

			// Update position with the checked velocity
			rigidBody.Item1.Position = axisAdder(rigidBody.Item1.Position, tickAxisVelocity);
			rigidBody.Item2.Velocity = velocity;
		}

		private float ResolveAxisCollision(float tickVelocity, BoundingBox rigidBox, BoundingBox fixedBox,
			Func<Vector3, float> axisGetter)
		{
			// Find the depth we're colliding on
			var depth = tickVelocity < 0
				? axisGetter(fixedBox.End) - axisGetter(rigidBox.Start)
				: axisGetter(fixedBox.Start) - axisGetter(rigidBox.End);

			// Adjust velocity so we won't overlap after update anymore
			tickVelocity += depth;

			return tickVelocity;
		}
	}
}