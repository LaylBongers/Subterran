using System;
using OpenTK;
using Subterran.Toolbox.SimplePhysics;
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
			Assert.True(entity.Transform.Position.X > 0.9f);
			Assert.True(entity.Transform.Position.X < 1.1f);
		}
	}
}
