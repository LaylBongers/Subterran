using System;
using System.Diagnostics;
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
						// Find any we collide on
						if (!PhysicsHelper.CheckCollision(targetBox, fixedBox))
							continue;

						var depth = FindCollisionDepth(tickVelocity, targetBox, fixedBox);

						// Resolve formula is:
						// velocity = prevVelocity * (1-depthScale)
						// depthScale = (1/|prevVelocity|*depth) <- This is the % of the velocity inside the collision
						if (depth.X < depth.Y && depth.X < depth.Z)
						{
							// X is smallest
							var xDepthScale = (1/Math.Abs(tickVelocity.X)*depth.X);
							Debug.Assert(!float.IsNaN(xDepthScale));
							tickVelocity.X = tickVelocity.X*(1 - xDepthScale);
							velocity.X = 0;
						}
						else if (depth.Y < depth.Z)
						{
							// Y is smallest
							var yDepthScale = (1/Math.Abs(tickVelocity.Y)*depth.Y);
							Debug.Assert(!float.IsNaN(yDepthScale));
							tickVelocity.Y = tickVelocity.Y*(1 - yDepthScale);
							velocity.Y = 0;
						}
						else
						{
							// Z is smallest
							var zDepthScale = (1/Math.Abs(tickVelocity.Z)*depth.Z);
							Debug.Assert(!float.IsNaN(zDepthScale));
							tickVelocity.Z = tickVelocity.Z*(1 - zDepthScale);
							velocity.X = 0;
						}

						// Update the values we worked with so the next collision check can be correct
						targetPosition = originalPosition + tickVelocity;
						targetBox = BoundingBox.FromPositionAndCollider(targetPosition, collider);
					}

					// Submit the changes to the rigidbody
					rigidBody.Item1.Position = originalPosition + tickVelocity;
					rigidBody.Item2.Velocity = velocity;
				}
			}
		}

		private Vector3 FindCollisionDepth(Vector3 velocity, BoundingBox movingBox, BoundingBox fixedBox)
		{
			var xDepth = velocity.X > 0
				? movingBox.End.X - fixedBox.Start.X
				: fixedBox.End.X - movingBox.Start.X;
			var yDepth = velocity.Y > 0
				? movingBox.End.Y - fixedBox.Start.Y
				: fixedBox.End.Y - movingBox.Start.Y;
			var zDepth = velocity.Z > 0
				? movingBox.End.Z - fixedBox.Start.Z
				: fixedBox.End.Z - movingBox.Start.Z;

			return new Vector3(xDepth, yDepth, zDepth);
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