using System;
using NSubstitute;
using Xunit;

namespace Subterran.Tests
{
	public class EntityTests
	{
		[Fact]
		public void GetComponent_MatchingComponent_ReturnsComponent()
		{
			// Arrange
			var component = Substitute.For<ComponentA>();
			var entity = new Entity {Components = {component}};

			// Act
			var result = entity.GetComponent<ComponentA>();

			// Assert
			Assert.Same(component, result);
		}

		[Fact]
		public void GetComponent_NoComponents_ReturnsNull()
		{
			// Arrange
			var entity = new Entity();

			// Act
			var result = entity.GetComponent<ComponentA>();

			// Assert
			Assert.Null(result);
		}

		[Fact]
		public void GetComponent_WrongComponents_ReturnsNull()
		{
			// Arrange
			var component = Substitute.For<ComponentB>();
			var entity = new Entity {Components = {component}};

			// Act
			var result = entity.GetComponent<ComponentA>();

			// Assert
			Assert.Null(result);
		}

		[Fact]
		public void GetComponents_OneMatchingOneWrong_ReturnsMatching()
		{
			// Arrange
			var componentA = Substitute.For<ComponentA>();
			var componentB = Substitute.For<ComponentB>();
			var entity = new Entity {Components = {componentA, componentB}};

			// Act
			var result = entity.GetComponent<ComponentA>();

			// Assert
			Assert.Same(componentA, result);
		}

		[Fact]
		public void Update_OneComponent_UpdatesComponent()
		{
			// Arrange
			var component = Substitute.For<EntityComponent>();
			var entity = new Entity {Components = {component}};

			// Act
			entity.Update(TimeSpan.FromSeconds(1));

			// Assert
			component.Received(1).Update(entity, TimeSpan.FromSeconds(1));
		}

		[Fact]
		public void Update_OneChild_UpdatesChildren()
		{
			// This assumes Update_OneComponent_UpdatesComponent passes

			// Arrange
			var component = Substitute.For<EntityComponent>();
			var childEntity = new Entity {Components = {component}};
			var entity = new Entity {Children = {childEntity}};

			// Act
			entity.Update(TimeSpan.FromSeconds(1));

			// Assert
			component.Received(1).Update(childEntity, TimeSpan.FromSeconds(1));
		}

		public abstract class ComponentA : EntityComponent
		{
		}

		public abstract class ComponentB : EntityComponent
		{
		}
	}
}