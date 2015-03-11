using System;
using NSubstitute;
using Xunit;

namespace Subterran.Tests
{
	[Trait("Type", "Unit")]
	[Trait("Namespace", "Subterran")]
	[Trait("Class", "Subterran.GameLoopTimer")]
	public class GameLoopTimerTests
	{
		[Fact]
		public void Constructor_NoExplicitRate_ExecutesTickOnce()
		{
			// Arrange
			var func = Substitute.For<Action>();
			var loop = new GameLoopTimer(_ => func());

			// Act
			loop.ExecuteTicks(TimeSpan.FromSeconds(1));

			// Assert
			func.Received(1).Invoke();
		}

		[Fact]
		public void Constructor_TwoPerSecond_ExecutesTickTwice()
		{
			// Arrange
			var func = Substitute.For<Action>();

			// Act
			var loop = new GameLoopTimer(_ => func(), 2);

			// Assert
			loop.ExecuteTicks(TimeSpan.FromSeconds(1));
			func.Received(2).Invoke();
		}

		[Fact]
		public void ExecuteTicks_MoreTimeThanFourTicks_RunsAtHigherTickLength()
		{
			// Arrange
			var func = Substitute.For<Action<TimeSpan>>();
			var loop = new GameLoopTimer(func, 4) {MaximumTicksPerExecution = 4};

			// Act
			loop.ExecuteTicks(TimeSpan.FromSeconds(2));

			// Assert
			func.Received(4).Invoke(Arg.Is<TimeSpan>(t => t > TimeSpan.FromSeconds(0.25)));
		}

		[Fact]
		public void ExecuteTicks_MoreTimeThanFourTicks_SetsIsRunningSlowTrue()
		{
			// Arrange
			var func = Substitute.For<Action>();
			var loop = new GameLoopTimer(_ => func(), 4) {MaximumTicksPerExecution = 4};

			// Act
			loop.ExecuteTicks(TimeSpan.FromSeconds(2));

			// Assert
			Assert.True(loop.IsRunningSlow);
		}
	}
}