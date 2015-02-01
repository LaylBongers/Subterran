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
			var rigidBodies = new List<Tuple<Entity, RigidbodyComponent>>();
			var fixedBodies = new List<BoundingBox>();

			// Get all rigid/fixed bodies that are a direct child of the entity we're attached to
			foreach (var entity in Entity.Children)
			{
				var rigidBody = entity.GetComponent<RigidbodyComponent>();
				if (rigidBody != null)
				{
					rigidBodies.Add(new Tuple<Entity, RigidbodyComponent>(entity, rigidBody));
				}

				var fixedBody = entity.GetComponent<FixedbodyComponent>();
				if (fixedBody != null)
				{
					fixedBodies.Add(BoundingBox.FromPositionAndCollider(
						entity.Position,
						fixedBody.Collider));
				}
			}
			
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

					// Find the new position for this physics tick and get the collider for it
					var velocity = rigidBody.Item2.Velocity;
					var tickVelocity = Timestep.PerSecond(velocity);
					var oldPosition = rigidBody.Item1.Position;
					var newPosition = oldPosition + tickVelocity;

					var rigidCollider = rigidBody.Item2.Collider;
					var rigidBox = BoundingBox.FromPositionAndCollider(newPosition, rigidCollider);
					
					// Check if our new position happens to collide
					// For this we have to go through every fixed body component we detected
					foreach (var fixedBox in fixedBodies)
					{
						if ( // Check if we collide on the axis we're checking
							rigidBox.Start.Y < fixedBox.End.Y && rigidBox.End.Y > fixedBox.Start.Y &&
							// Make sure we also collide on the perpendicular axes
							rigidBox.Start.X < fixedBox.End.X && rigidBox.End.X > fixedBox.Start.X &&
							rigidBox.Start.Z < fixedBox.End.Z && rigidBox.End.Z > fixedBox.Start.Z)
						{
							// Adjust velocity so we won't overlap after update anymore
							var difference = velocity.Y < 0
								? fixedBox.End.Y - rigidBox.Start.Y
								: fixedBox.Start.Y - rigidBox.End.Y;
							tickVelocity += new Vector3(0, difference, 0);

							// Reset the velocity of the rigidbody so it doesn't keep moving
							velocity.Y = 0;

							// Update the position based on our new velocity
							newPosition = oldPosition + tickVelocity;
							rigidBox = BoundingBox.FromPositionAndCollider(newPosition, rigidCollider);
						}
					}

					// Update their position with the set velocity
					rigidBody.Item1.Position = newPosition;
					rigidBody.Item2.Velocity = velocity;
				}
			}
		}
	}
}