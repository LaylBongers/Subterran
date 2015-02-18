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
		private bool _climbing;
		private Vector3 _climbTarget;
		private RigidbodyComponent _rigidbody;

		public void Initialize()
		{
			_rigidbody = Entity.RequireComponent<RigidbodyComponent>();
		}

		public void Update(TimeSpan elapsed)
		{
			if (_climbing)
			{
				Climb(elapsed);
			}
			else
			{
				DetectClimbing();
			}
		}

		private void Climb(TimeSpan elapsed)
		{
			var distance = _climbTarget - Entity.Transform.Position;
			var tickDistance = distance;
			tickDistance.NormalizeFast();
			tickDistance *= 4f; // Speed
			tickDistance *= (float) elapsed.TotalSeconds;

			// If the distance is lower than the tickDistance, this is the last move tick
			if (Math.Abs(distance.X) <= Math.Abs(tickDistance.X))
			{
				Entity.Transform.Position = _climbTarget;
				_climbing = false;
				_rigidbody.Enabled = true;
			}
			else
			{
				Entity.Transform.Position += tickDistance;
			}
		}

		private void DetectClimbing()
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
			var allBoxes = fixedBoxes.Concat(smartBoxes).ToList();

			// Actually perform the autoclimb checks on the axes
			foreach (var fixedBox in fixedBoxes.Concat(allBoxes))
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
				var climbOffset = new Vector3(tickVelocity.X, climbAmount, tickVelocity.Z);
				var climbedBox = currentBox.Translate(climbOffset);

				// Make sure the new box isn't taken up by any fixed boxes
				if (!IsAreaFree(climbedBox, allBoxes))
					continue;

				// The new position is valid, set it as a target so we can move to it
				_climbTarget = Entity.Transform.Position + climbOffset;
				_climbing = true;
				_rigidbody.Enabled = false;
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