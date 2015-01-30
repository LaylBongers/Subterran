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

		public PlayerMoveComponent()
		{
			// Default values
			Speed = 5;
		}

		public float Speed { get; set; }

		public void Initialize()
		{
			_rigidbody = Entity.RequireComponent<RigidbodyComponent>();
		}

		public void Update(TimeSpan elapsed)
		{
			var state = Keyboard.GetState();
			var velocity = _rigidbody.Velocity;

			velocity.X = 0;
			if (state[Key.D])
			{
				velocity.X += 1*Speed;
			}
			if (state[Key.A])
			{
				velocity.X += -1*Speed;
			}

			_rigidbody.Velocity = velocity;
		}
	}
}