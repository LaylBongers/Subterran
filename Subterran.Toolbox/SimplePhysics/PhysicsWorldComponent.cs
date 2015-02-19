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

		public event EventHandler Updated = (s, e) => { };

		public void Update(TimeSpan elapsed)
		{
			// Optimization Note: Allocating tuples might cause GC issues, perhaps replace with structs?
			var rigidBodies = PhysicsHelper.FindRigidBodies(Entity);
			var fixedBoxes = PhysicsHelper.FindFixedBoundingBoxes(Entity);

			// We need to do a physics update once per time step
			var updated = false;
			_accumulator += elapsed;
			while (_accumulator > Timestep)
			{
				updated = true;
				_accumulator -= Timestep;

				// Go through all of the rigid bodies 
				foreach (var rigidBody in rigidBodies)
				{
					// Put the original information in helper variables
					var position = rigidBody.Item1.Transform.Position;
					var velocity = rigidBody.Item2.Velocity;
					var collider = rigidBody.Item2.Collider;

					// Add a bit of gravity
					velocity += elapsed.PerSecond(rigidBody.Item2.Gravity);

					// If we don't have a collider we can't collide
					if (collider != null)
					{
						// Move the rigid body keeping collisions in mind
						MoveOnAxis(ref position, ref velocity, collider, fixedBoxes, StVector.GetX, StVector.SetX);
						MoveOnAxis(ref position, ref velocity, collider, fixedBoxes, StVector.GetY, StVector.SetY);
						MoveOnAxis(ref position, ref velocity, collider, fixedBoxes, StVector.GetZ, StVector.SetZ);
					}
					else
					{
						// Move the rigid body without collisions
						position += elapsed.PerSecond(velocity);
					}

					// Submit the changes to the rigidbody
					rigidBody.Item1.Transform.Position = position;
					rigidBody.Item2.Velocity = velocity;
				}
			}

			if (updated)
			{
				Updated(this, EventArgs.Empty);
			}
		}

		private void MoveOnAxis(ref Vector3 position, ref Vector3 velocity, CubeCollider collider,
			IEnumerable<BoundingBox> fixedBoxes, StVector.AxisGetFunc axisGetter, StVector.AxisSetAction axisSetter)
		{
			// Find how much we'll move this tick
			var tickVelocity = Timestep.PerSecond(axisGetter(velocity));

			// Get a bounding box encompassing the start and end of the rigidbody on this axis
			// By using the encompassing bounding box, we can avoid passing through blocks by moving fast
			var original = BoundingBox.FromPositionAndCollider(position, collider);
			axisSetter(ref position, axisGetter(position) + tickVelocity); // Velocity is added to position here
			var target = BoundingBox.FromPositionAndCollider(position, collider);
			var encompassingBox = BoundingBox.Encompassing(original, target);

			// Get all the smart broadphase collected colliders
			var smartBoxes = PhysicsHelper.FindSmartBoundingBoxes(Entity, encompassingBox);

			foreach (var fixedBox in fixedBoxes.Concat(smartBoxes))
			{
				// If there is no collision, we've got nothing to do
				if (!PhysicsHelper.CheckCollision(target, fixedBox))
					continue;

				// Find the depth on this axis
				float depth;
				if (tickVelocity > 0) // Do not use Velocity here, that will be reset
				{
					depth = axisGetter(target.End) - axisGetter(fixedBox.Start);
				}
				else
				{
					depth = axisGetter(target.Start) - axisGetter(fixedBox.End);
				}

				// Add a bit to the depth to avoid floating point errors
				depth = StMath.AddSigned(depth, 0.0001f);

				// Remove the depth from the position to resolve the collision
				axisSetter(ref position, axisGetter(position) - depth);

				// Reset velocity to prevent the collision from occurring again
				axisSetter(ref velocity, 0);

				// Recalculate the target for further checks
				target = BoundingBox.FromPositionAndCollider(position, collider);
			}
		}
	}
}