using System;
using OpenTK;
using OpenTK.Input;
using Subterran;
using Subterran.Toolbox;
using Subterran.Toolbox.SimplePhysics;

namespace VoxelWorld
{
	public class AimPlaceBlockComponent : EntityComponent, IUpdatable
	{
		private Vector3 _targetPosition;
		public Entity TargetReference { get; set; }

		public void Update(TimeSpan elapsed)
		{
			UpdateRaycast();

			var state = Mouse.GetState();
			if (state.LeftButton == ButtonState.Pressed)
			{
				PlaceBlock();
			}
		}

		private void UpdateRaycast()
		{
			// Get the actual position of our camera relative to the physics world
			var position = Entity.Parent.Transform.Position + Entity.Transform.Position;

			// Get the direction the raycast should go at
			var rotation = Entity.Transform.Rotation;
			var direction = new Vector3(
				(float) Math.Sin(-rotation.Y) * (float)Math.Cos(rotation.X),
				(float) Math.Sin(rotation.X),
				(float) -Math.Cos(rotation.Y) * (float)Math.Cos(rotation.X));
			
			// Get the physics world which we need to do the raycast
			var physics = Entity.Parent.Parent.GetComponent<PhysicsWorldComponent>();

			// TODO: Remove this test
			_targetPosition = position + direction;

			// Perform the actual raycast
			//physics.Raycast(position, )
		}

		private void PlaceBlock()
		{
			TargetReference.Transform.Position = _targetPosition;
		}
	}
}