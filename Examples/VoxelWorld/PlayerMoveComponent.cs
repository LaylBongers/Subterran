using System;
using OpenTK;
using OpenTK.Input;
using Subterran;
using Subterran.Toolbox;
using Subterran.Toolbox.SimplePhysics;
using Subterran.WorldState;

namespace VoxelWorld
{
	internal class PlayerMoveComponent : EntityComponent, IInitializable, IUpdatable
	{
		private readonly StandardWindowService _window;
		private PlayerAutoclimbComponent _autoclimb;
		private bool _flyMode;
		private SensorComponent _jumpSensor;
		private Vector3 _normalGravity;
		private Vector2 _previousPosition;
		private RigidbodyComponent _rigidbody;
		private SensorComponent _uncrouchSensor;
		private bool _wasCrouched;
		private bool _wasJumpDown;

		public PlayerMoveComponent(StandardWindowService window)
		{
			// Default values
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
			_rigidbody = Entity.RequireOne<RigidbodyComponent>();
			_jumpSensor = Entity.RequireOne<SensorComponent>(s => s.Name == "JumpSensor");
			_uncrouchSensor = Entity.RequireOne<SensorComponent>(s => s.Name == "UncrouchSensor");
			_autoclimb = Entity.GetOne<PlayerAutoclimbComponent>();

			_normalGravity = _rigidbody.Gravity;

			_window.ShowCursor = false;
			_window.ClipCursor = true;
		}

		public void Update(TimeSpan elapsed)
		{
			var state = Keyboard.GetState();

			UpdateRotation();

			if (_flyMode)
			{
				// If control is pressed while near the ground we need to land
				if (state.IsKeyDown(Key.ControlLeft) && _jumpSensor.IsTriggered)
				{
					_flyMode = false;
					return;
				}

				_rigidbody.Gravity = new Vector3();
				UpdateFlyPosition(state);
			}
			else
			{
				_rigidbody.Gravity = _normalGravity;

				UpdateCrouch(state);
				UpdatePosition(state);
				UpdateJump(state);
			}
		}

		private void UpdateFlyPosition(KeyboardState state)
		{
			var targetDirection = GetDirectionVector(state);

			var speedMultiplier = state.IsKeyDown(Key.ShiftLeft)
				? FastSpeed
				: Speed;

			targetDirection.NormalizeFast();
			var targetVelocity = targetDirection*speedMultiplier;

			var velocity = _rigidbody.Velocity;
			velocity.X = targetVelocity.X;
			velocity.Y = targetVelocity.Y;
			velocity.Z = targetVelocity.Z;
			_rigidbody.Velocity = velocity;
		}

		private void UpdateCrouch(KeyboardState state)
		{
			var camPos = CameraEntity.Transform.Position;
			var clipSize = _rigidbody.Collider.Size;


			if ( // Crouch when left control is pressed
				state.IsKeyDown(Key.ControlLeft) ||
				// Prevent uncrouching while doing that would clip us with something
				(_wasCrouched && _uncrouchSensor.IsTriggered) ||
				// Don't uncrouch in the middle of a climb as this could cause us to clip into something
				(_wasCrouched && _autoclimb != null && _autoclimb.IsClimbing))
			{
				// Crouching
				camPos.Y = 0.75f;
				clipSize.Y = 0.8f;
				_wasCrouched = true;
			}
			else
			{
				// Standing
				camPos.Y = 1.5f;
				clipSize.Y = 1.8f;
				_wasCrouched = false;
			}

			CameraEntity.Transform.Position = camPos;
			_rigidbody.Collider.Size = clipSize;
		}

		private void UpdateRotation()
		{
			// Get how much the mouse has changed
			var state = Mouse.GetState();
			var deltaPosition = new Vector2(
				state.X - _previousPosition.X,
				state.Y - _previousPosition.Y);
			_previousPosition = new Vector2(state.X, state.Y);

			// Update the rotation of the entity based on the difference in mouse position
			var rotation = CameraEntity.Transform.Rotation.Xy + (-deltaPosition.Yx*0.0015f);
			CameraEntity.Transform.Rotation = new Vector3(
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
			var rotationMatrix =
				Matrix4.CreateRotationX(CameraEntity.Transform.Rotation.X)*
				Matrix4.CreateRotationY(CameraEntity.Transform.Rotation.Y);

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
			if (state.IsKeyDown(Key.Space))
			{
				if (!_wasJumpDown && !_jumpSensor.IsTriggered)
				{
					// If jump was re-pressed in the sky
					_flyMode = true;
					StLogging.Info("Fly mode triggered!");
				}

				if (_jumpSensor.IsTriggered)
				{
					var velocity = _rigidbody.Velocity;
					velocity.Y = JumpHeight;
					_rigidbody.Velocity = velocity;
					_wasJumpDown = true;
				}
			}
			else
			{
				_wasJumpDown = false;
			}
		}
	}
}