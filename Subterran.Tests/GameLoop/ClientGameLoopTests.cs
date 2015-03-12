using System;
using NSubstitute;
using Subterran.GameLoop;
using Xunit;

namespace Subterran.Tests.GameLoop
{
	[Trait("Type", "Unit")]
	[Trait("Namespace", "Subterran.GameLoop")]
	[Trait("Class", "Subterran.GameLoop.ClientGameLoop")]
	public class ClientGameLoopTests
	{
		[Fact]
		public void UpdateTick_ProcessesWindowEvents()
		{
			// Arrange
			var window = Substitute.For<IWindowService>();
			var loop = new ClientGameLoop(new GameInstance(new GameInfo()), window);

			// Act
			loop.UpdateTick(TimeSpan.FromSeconds(0.1f));

			// Assert
			window.Received(1).ProcessEvents();
		}
	}
}