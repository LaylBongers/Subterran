using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK;
using Subterran;
using Subterran.Toolbox;
using Subterran.Toolbox.SimplePhysics;

namespace VoxelWorld
{
	internal class PlayerAutoclimbComponent : EntityComponent, IInitializable, IUpdatable
	{
		private RigidbodyComponent _rigidbody;

		public void Initialize()
		{
			_rigidbody = Entity.RequireComponent<RigidbodyComponent>();
		}

		public void Update(TimeSpan elapsed)
		{
			var tickVelocity = _rigidbody.Velocity*0.01f;
			var position = Entity.Transform.Position;
			var targetPosition = position + new Vector3(tickVelocity.X, 0, tickVelocity.Z);

			var currentBox = BoundingBox.FromPositionAndCollider(position, _rigidbody.Collider);
			var targetBox = BoundingBox.FromPositionAndCollider(targetPosition, _rigidbody.Collider);

			// Encompassing needs to be 0.6f higher since we might need to raise upwards
			var encompassing = BoundingBox.Encompassing(currentBox, targetBox);
			encompassing.End += new Vector3(0, 0.6f, 0);

			var fixedBoxes = PhysicsHelper.FindFixedBoundingBoxes(Entity.Parent);
			var smartBoxes = PhysicsHelper.FindSmartBoundingBoxes(Entity.Parent, encompassing);

			// Actually perform the autoclimb checks on the axes
			foreach (var fixedBox in fixedBoxes.Concat(smartBoxes))
			{
				if (!PhysicsHelper.CheckCollision(targetBox, fixedBox))
					continue;

				// Get the amount we need to go up to climb the collided box
				// Add a small floating point error compensation
				var climbAmount = fixedBox.End.Y - targetBox.Start.Y + 0.001f;

				// We can only climb up 0.6f in height
				if (climbAmount > 0.6f)
					continue;

				// Get the box we'll end up at if we translate to the top of the collided box
				var climbedBox = targetBox.Translate(new Vector3(0, climbAmount, 0));

				// Make sure the new box isn't taken up by any fixed boxes
				if (!IsAreaFree(climbedBox, fixedBoxes))
					continue;

				// The new position is valid, apply it and we're done
				Entity.Transform.Position += new Vector3(tickVelocity.X, climbAmount, tickVelocity.Z);
				return;
			}
		}

		private static bool IsAreaFree(BoundingBox box, IEnumerable<BoundingBox> fixedBoxes)
		{
			foreach (var fixedBox in fixedBoxes)
			{
				if (PhysicsHelper.CheckCollision(box, fixedBox))
					return false;
			}

			return true;
		}
	}
}