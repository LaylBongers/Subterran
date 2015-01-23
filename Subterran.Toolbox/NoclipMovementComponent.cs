using System;
using OpenTK;
using OpenTK.Input;

namespace Subterran.Toolbox
{
	public class NoclipMovementComponent : EntityComponent, IInitializable, IUpdatable
	{
		private readonly Window _window;
		private Vector2 _previousPosition;

		public NoclipMovementComponent(Window window)
		{
			// Default values, once C# 6.0 rolls around we can do this inline
			Speed = 5.0f;
			FastSpeed = 10.0f;

			_window = window;
		}

		public float Speed { get; set; }
		public float FastSpeed { get; set; }

		public void Initialize()
		{
			_window.ShowCursor = false;
			_window.ClipCursor = true;
		}

		public void Update(TimeSpan elapsed)
		{
			// We can't handle anything related to input if the window isn't focused
			if (!_window.IsFocused)
				return;

			UpdateRotation();
			UpdatePosition(elapsed);
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
			var rotation = Entity.Rotation.Xy + (-deltaPosition.Yx*0.0015f);
			Entity.Rotation = new Vector3(
				MathHelper.Clamp(rotation.X, -StMath.Tau*0.25f, StMath.Tau*0.25f),
				rotation.Y, 0);
		}

		private void UpdatePosition(TimeSpan elapsed)
		{
			var state = Keyboard.GetState();
			var targetDirection = GetDirectionVector(state);

			var speedMultiplier = state.IsKeyDown(Key.ShiftLeft)
				? FastSpeed
				: Speed;

			targetDirection.NormalizeFast();
			Entity.Position += elapsed.PerSecond(
				targetDirection*speedMultiplier);
		}

		private Vector3 GetDirectionVector(KeyboardState state)
		{
			var rotationMatrix = Entity.Matrix;

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
	}
}