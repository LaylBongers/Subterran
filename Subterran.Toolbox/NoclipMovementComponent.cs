using System;
using OpenTK;
using OpenTK.Input;
using Subterran.OpenTK;

namespace Subterran.Toolbox
{
	public class NoclipMovementComponent : EntityComponent
	{
		public NoclipMovementComponent(InputManager input)
		{
			// Default values, once C# 6.0 rolls around we can do this inline
			Speed = 1.0f;
			FastSpeed = 2.0f;

			input.AimChange += InputOnAimChange;
		}

		public float Speed { get; set; }
		public float FastSpeed { get; set; }

		public override void Update(TimeSpan elapsed)
		{
			var rotationMatrix =
				Matrix4.CreateRotationX(Entity.Rotation.X)*
				Matrix4.CreateRotationY(Entity.Rotation.Y);

			var backwards = Vector3.Transform(Vector3.UnitZ, rotationMatrix);
			var right = Vector3.Transform(Vector3.UnitX, rotationMatrix);

			// TODO: Move this into the Subterran.OpenTK library
			var keyboard = Keyboard.GetState();
			var targetDirection = new Vector3();

			if (keyboard.IsKeyDown(Key.S))
				targetDirection += backwards;
			if (keyboard.IsKeyDown(Key.W))
				targetDirection -= backwards;
			if (keyboard.IsKeyDown(Key.D))
				targetDirection += right;
			if (keyboard.IsKeyDown(Key.A))
				targetDirection -= right;

			targetDirection.NormalizeFast();
			Entity.Position += elapsed.PerSecond(
				targetDirection*(
					keyboard.IsKeyDown(Key.ShiftLeft)
						? FastSpeed
						: Speed));
		}

		private void InputOnAimChange(object sender, AimEventArgs e)
		{
			var rotation = Entity.Rotation.Xy + (-e.Delta.Yx*0.0015f);
			Entity.Rotation = new Vector3(
				MathHelper.Clamp(rotation.X, -MathHelper.PiOver2, MathHelper.PiOver2),
				rotation.Y, 0);
		}
	}
}