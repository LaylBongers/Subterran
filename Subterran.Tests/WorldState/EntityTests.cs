using System;
using NSubstitute;
using Subterran.WorldState;
using Xunit;

namespace Subterran.Tests.WorldState
{
	[Trait("Type", "Unit")]
	[Trait("Namespace", "Subterran.WorldState")]
	[Trait("Class", "Subterran.WorldState.EntityTests")]
	public class EntityTests
	{
		[Fact]
		public void GetComponent_MatchingComponent_ReturnsComponent()
		{
			// Arrange
			var component = Substitute.For<ComponentA>();
			var entity = new Entity {Components = {component}};

			// Act
			var result = entity.GetOne<ComponentA>();

			// Assert
			Assert.Same(component, result);
		}

		[Fact]
		public void GetComponent_NoComponents_ReturnsNull()
		{
			// Arrange
			var entity = new Entity();

			// Act
			var result = entity.GetOne<ComponentA>();

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
			var result = entity.GetOne<ComponentA>();

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
			var result = entity.GetOne<ComponentA>();

			// Assert
			Assert.Same(componentA, result);
		}

		[Fact]
		public void ForEach_OneCallableComponent_CallableComponent()
		{
			// Arrange
			var component = Substitute.For<CallableComponent>();
			var entity = new Entity {Components = {component}};

			// Act
			entity.ForEach<ICallable>(e => e.Call(5));

			// Assert
			component.Received(1).Call(5);
		}

		[Fact]
		public void ForEach_OneCallableChild_CallableChildren()
		{
			// This assumes Update_OneComponent_UpdatesComponent passes

			// Arrange
			var component = Substitute.For<CallableComponent>();
			var childEntity = new Entity {Components = {component}};
			var entity = new Entity {Children = {childEntity}};

			// Act
			entity.ForEach<ICallable>(e => e.Call(5));

			// Assert
			component.Received(1).Call(5);
		}

		[Fact]
		public void Parent_HasParent_ReturnsParent()
		{
			// Arrange
			var parent = new Entity();
			var child = new Entity();

			// Act
			parent.Children.Add(child);

			// Assert
			Assert.Same(parent, child.Parent);
		}

		[Fact]
		public void Parent_RemovedFromChildren_ReturnsNull()
		{
			// Arrange
			var parent = new Entity();
			var child = new Entity();

			// Act
			parent.Children.Add(child);
			parent.Children.Remove(child);

			// Assert
			Assert.Null(child.Parent);
		}

		[Fact]
		public void RequireComponent_MatchingComponent_ReturnsComponent()
		{
			// Arrange
			var component = Substitute.For<ComponentA>();
			var entity = new Entity {Components = {component}};

			// Act
			var result = entity.RequireOne<ComponentA>();

			// Assert
			Assert.Same(component, result);
		}

		[Fact]
		public void RequireComponent_NoComponents_ThrowsException()
		{
			// Arrange
			var entity = new Entity();

			// Act & Assert
			Assert.Throws<InvalidOperationException>(() => entity.RequireOne<ComponentA>());
		}

		[Fact]
		public void RequireComponent_NoComponents_ThrowsExceptionWithComponentName()
		{
			// Arrange
			var entity = new Entity();

			// Act
			var ex = new InvalidOperationException("No exception happened :C");
			try
			{
				entity.RequireOne<ComponentA>();
			}
			catch (InvalidOperationException e)
			{
				ex = e;
			}

			// Assert
			Assert.Contains("ComponentA", ex.Message);
		}

		[Fact]
		public void RequireComponent_MatchingComponentWithMatchingPredicate_ReturnsComponent()
		{
			// Arrange
			var component = Substitute.For<ComponentA>();
			component.Data = "Matching";
			var entity = new Entity {Components = {component}};

			// Act
			var result = entity.RequireOne<ComponentA>(c => c.Data == "Matching");

			// Assert
			Assert.Same(component, result);
		}

		[Fact]
		public void RequireComponent_MatchingComponentWithWrongPredicate_ThrowsException()
		{
			// Arrange
			var component = Substitute.For<ComponentA>();
			component.Data = "Wrong";
			var entity = new Entity {Components = {component}};

			// Act & Assert
			Assert.Throws<InvalidOperationException>(() => entity.RequireOne<ComponentA>(c => c.Data == "Matching"));
		}

		public abstract class ComponentA : EntityComponent
		{
			public string Data { get; set; }
		}

		public abstract class ComponentB : EntityComponent
		{
		}

		public abstract class CallableComponent : EntityComponent, ICallable
		{
			public abstract void Call(int num);
		}

		public interface ICallable
		{
			void Call(int num);
		}
	}
}