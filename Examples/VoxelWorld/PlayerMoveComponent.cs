using System;
using OpenTK;
using OpenTK.Input;
using Subterran;
using Subterran.Toolbox;
using Subterran.Toolbox.SimplePhysics;

namespace VoxelWorld
{
	internal class PlayerMoveComponent : EntityComponent, IInitializable, IUpdatable
	{
		private readonly Window _window;
		private SensorComponent _jumpSensor;
		private Vector2 _previousPosition;
		private RigidbodyComponent _rigidbody;

		public PlayerMoveComponent(Window window)
		{
			// Default values, once C# 6.0 rolls around we can do this inline
			Speed = 5.0f;
			FastSpeed = 10.0f;

			_window = window;
		}

		public float Speed { get; set; }
		public float FastSpeed { get; set; }
		public float JumpHeight { get; set; }
		public Entity CameraEntity { get; set; }

		public void Initialize()
		{
			_rigidbody = Entity.RequireComponent<RigidbodyComponent>();
			_jumpSensor = Entity.RequireComponent<SensorComponent>(s => s.Name == "JumpSensor");

			_window.ShowCursor = false;
		}

		public void Update(TimeSpan elapsed)
		{
			var state = Keyboard.GetState();

			UpdateRotation();
			UpdatePosition(state);
			UpdateJump(state);
		}

		private void UpdateRotation()
		{
			// Get how much the mouse has changed
			var state = Mouse.GetState();
			var deltaPosition = new Vector2(
				state.X - _previousPosition.X,
				state.Y - _previousPosition.Y);

			// Reset the mouse to the middle of the screen
			Mouse.SetPosition(
				_window.Bounds.Left + (_window.Bounds.Width/2),
				_window.Bounds.Top + (_window.Bounds.Height/2));

			// Store the position of the mouse currently so we can get the delta again next update
			state = Mouse.GetState();
			_previousPosition = new Vector2(state.X, state.Y);

			// Update the rotation of the entity based on the difference in mouse position
			var rotation = CameraEntity.Rotation.Xy + (-deltaPosition.Yx * 0.0015f);
			CameraEntity.Rotation = new Vector3(
				MathHelper.Clamp(rotation.X, -StMath.Tau*0.25f, StMath.Tau*0.25f),
				rotation.Y, 0);
		}

		private void UpdatePosition(KeyboardState state)
		{
			var targetDirection = GetDirectionVector(state);
			targetDirection.Y = 0;

			var speedMultiplier = state.IsKeyDown(Key.ShiftLeft)
				? FastSpeed
				: Speed;

			targetDirection.NormalizeFast();
			var targetVelocity = targetDirection*speedMultiplier;

			var velocity = _rigidbody.Velocity;
			velocity.X = targetVelocity.X;
			velocity.Z = targetVelocity.Z;
			_rigidbody.Velocity = velocity;
		}

		private Vector3 GetDirectionVector(KeyboardState state)
		{
			var rotationMatrix = CameraEntity.Matrix;

			var backwards = Vector3.TransformVector(Vector3.UnitZ, rotationMatrix);
			var right = Vector3.TransformVector(Vector3.UnitX, rotationMatrix);

			var targetDirection = new Vector3();

			if (state.IsKeyDown(Key.S))
				targetDirection += backwards;
			if (state.IsKeyDown(Key.W))
				targetDirection -= backwards;
			if (state.IsKeyDown(Key.D))
				targetDirection += right;
			if (state.IsKeyDown(Key.A))
				targetDirection -= right;

			return targetDirection;
		}

		private void UpdateJump(KeyboardState state)
		{
			if (state.IsKeyDown(Key.Space) && _jumpSensor.CheckTriggered())
			{
				var velocity = _rigidbody.Velocity;
				velocity.Y = JumpHeight;
				_rigidbody.Velocity = velocity;
			}
		}
	}
}