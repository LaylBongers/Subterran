using NSubstitute;
using Subterran.WorldState;
using Xunit;

namespace Subterran.Tests.WorldState
{
	public class EntityComponentTests
	{
		[Fact]
		public void CheckInitialize_InitializableComponent_Initializes()
		{
			// Arrange
			var component = Substitute.For<InitializableComponent>();

			// Act
			component.CheckInitialize();

			// Assert
			component.Received(1).Initialize();
		}

		public abstract class InitializableComponent : EntityComponent, IInitializable
		{
			public abstract void Initialize();
		}
	}
}