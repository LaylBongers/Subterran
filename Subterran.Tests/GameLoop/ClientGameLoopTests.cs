using System;
using NSubstitute;
using OpenTK.Input;
using Subterran.GameLoop;
using Subterran.Input;
using Subterran.WorldState;
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

			var loop = new ClientGameLoop(new GameInstance(new GameInfo()),
				window, Substitute.For<IWorldStateService>(), Substitute.For<IInputService>());

			// Act
			loop.UpdateTick(TimeSpan.FromSeconds(0.1f));

			// Assert
			window.Received(1).ProcessEvents();
		}

		[Fact]
		public void UpdateTick_EscHeldDown_StopsGame()
		{
			// Arrange
			var input = Substitute.For<IInputService>();
			input.IsKeyDown(Key.Escape).Returns(true);

			var loop = new ClientGameLoop(new GameInstance(new GameInfo()),
				Substitute.For<IWindowService>(), Substitute.For<IWorldStateService>(), input);

			var wasStopped = false;
			loop.Stopped += (s, e) => wasStopped = true;

			// Act
			loop.UpdateTick(TimeSpan.FromSeconds(0.1f));

			// Assert
			Assert.True(wasStopped);
		}
	}
}