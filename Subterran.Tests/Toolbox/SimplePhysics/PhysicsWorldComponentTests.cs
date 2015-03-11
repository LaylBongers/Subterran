using System;
using OpenTK;
using Subterran.Toolbox.SimplePhysics;
using Subterran.WorldState;
using Xunit;

namespace Subterran.Tests.Toolbox.SimplePhysics
{
	public class PhysicsWorldComponentTests
	{
		[Fact]
		public void Update_RightVelocity_MovesRight()
		{
			// Arrange
			var velocity = new Vector3(1, 0, 0);
			var entity = new Entity
			{
				Components =
				{
					new RigidbodyComponent
					{
						Velocity = velocity
					}
				}
			};
			var physicsWorld = new PhysicsWorldComponent();
			var world = new Entity
			{
				Children = {entity},
				Components = {physicsWorld}
			};

			// Act
			physicsWorld.Update(TimeSpan.FromSeconds(1.0));

			// Assert
			var value = entity.Transform.Position.X;
			Assert.True(value > 0.9f, "Expected > 0.9f, Actual value: " + value);
			Assert.True(value < 1.1f, "Expected < 1.1f, Actual value: " + value);
		}
	}
}
