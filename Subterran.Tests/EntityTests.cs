using System;
using System.Collections.ObjectModel;
using NSubstitute;
using Xunit;

namespace Subterran.Tests
{
	public class EntityTests
	{
		[Fact]
		public void GetComponent_MatchingComponent_ReturnsComponent()
		{
			GetT_MatchingT_ReturnsT(
				e => e.Components,
				e => e.GetComponent<ComponentA>());
		}


		[Fact]
		public void GetBehavior_MatchingBehavior_ReturnsBehavior()
		{
			GetT_MatchingT_ReturnsT(
				e => e.Behaviors,
				e => e.GetBehavior<BehaviorA>());
		}

		private void GetT_MatchingT_ReturnsT<TBase, T>(Func<Entity, Collection<TBase>> collection, Func<Entity, T> getter)
			where T : class, TBase
		{
			// Arrange
			var component = Substitute.For<T>();
			var entity = new Entity();
			collection(entity).Add(component);

			// Act
			var result = getter(entity);

			// Assert
			Assert.Same(component, result);
		}

		[Fact]
		public void GetComponent_NoComponents_ReturnsNull()
		{
			GetT_NoComponents_ReturnsNull(e => e.GetComponent<ComponentA>());
		}

		[Fact]
		public void GetBehavior_NoBehaviors_ReturnsNull()
		{
			GetT_NoComponents_ReturnsNull(e => e.GetBehavior<BehaviorA>());
		}

		private void GetT_NoComponents_ReturnsNull<T>(Func<Entity, T> getter)
		{
			// Arrange
			var entity = new Entity();

			// Act
			var result = getter(entity);

			// Assert
			Assert.Null(result);
		}

		[Fact]
		public void GetComponent_WrongComponent_ReturnsNull()
		{
			GetT_TWrong_ReturnsNull<IEntityComponent, ComponentA, ComponentB>(
				e => e.Components,
				e => e.GetComponent<ComponentA>());
		}

		[Fact]
		public void GetBehavior_WrongBehavior_ReturnsNull()
		{
			GetT_TWrong_ReturnsNull<EntityBehavior, BehaviorA, BehaviorB>(
				e => e.Behaviors,
				e => e.GetBehavior<BehaviorA>());
		}

		private void GetT_TWrong_ReturnsNull<TBase, T, TWrong>(Func<Entity, Collection<TBase>> collection, Func<Entity, T> getter)
			where TWrong : class, TBase
			where T : class, TBase
		{
			// Arrange
			var component = Substitute.For<TWrong>();
			var entity = new Entity();
			collection(entity).Add(component);

			// Act
			var result = getter(entity);

			// Assert
			Assert.Null(result);
		}

		[Fact]
		public void GetComponent_OneMatchingOneWrong_ReturnsMatching()
		{
			GetT_OneTOneTWrong_ReturnsT<IEntityComponent, ComponentA, ComponentB>(
				e => e.Components,
				e => e.GetComponent<ComponentA>());
		}

		[Fact]
		public void GetBehavior_OneMatchingOneWrong_ReturnsMatching()
		{
			GetT_OneTOneTWrong_ReturnsT<EntityBehavior, BehaviorA, BehaviorB>(
				e => e.Behaviors,
				e => e.GetBehavior<BehaviorA>());
		}

		private void GetT_OneTOneTWrong_ReturnsT<TBase, T, TWrong>(Func<Entity, Collection<TBase>> collection, Func<Entity, T> getter)
			where TWrong : class, TBase
			where T : class, TBase
		{
			// Arrange
			var componentA = Substitute.For<T>();
			var componentB = Substitute.For<TWrong>();
			var entity = new Entity();
			collection(entity).Add(componentA);
			collection(entity).Add(componentB);

			// Act
			var result = getter(entity);

			// Assert
			Assert.Same(componentA, result);
		}

		public abstract class BehaviorA : EntityBehavior
		{
		}

		public abstract class BehaviorB : EntityBehavior
		{
		}

		public abstract class ComponentA : IEntityComponent
		{
		}

		public abstract class ComponentB : IEntityComponent
		{
		}
	}
}