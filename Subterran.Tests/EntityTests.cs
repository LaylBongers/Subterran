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

		public abstract class ComponentA : EntityComponent
		{
		}

		public abstract class ComponentB : EntityComponent
		{
		}
	}
}