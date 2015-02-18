using System;
using OpenTK.Input;
using Subterran;
using Subterran.Toolbox;
using Subterran.Toolbox.SimplePhysics;

namespace Platformer
{
	internal class PlayerMoveComponent : EntityComponent, IInitializable, IUpdatable
	{
		private RigidbodyComponent _rigidbody;
		private SensorComponent _jumpSensor;

		public float Speed { get; set; }
		public float JumpHeight { get; set; }

		public void Initialize()
		{
			_rigidbody = Entity.RequireComponent<RigidbodyComponent>();
			_jumpSensor = Entity.RequireComponent<SensorComponent>(s => s.Name == "JumpSensor");
		}

		public void Update(TimeSpan elapsed)
		{
			var state = Keyboard.GetState();
			var velocity = _rigidbody.Velocity;

			// Left/Right Movement
			velocity.X = 0;
			if (state[Key.D])
			{
				velocity.X += 1*Speed;
			}
			if (state[Key.A])
			{
				velocity.X += -1*Speed;
			}

			// Jumping
			if (state[Key.W] && _jumpSensor.IsTriggered)
			{
				velocity.Y = JumpHeight;
			}

			_rigidbody.Velocity = velocity;
		}
	}
}