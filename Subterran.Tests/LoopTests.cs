using System;
using NSubstitute;
using Xunit;

namespace Subterran.Tests
{
	public class LoopTests
	{
		[Fact]
		public void ExecuteTicks_DeltaOfOncePerSecond_ExecutesTickOnce()
		{
			// Arrange
			var func = Substitute.For<Action>();
			var loop = Loop
				.ThatCalls(func)
				.WithDeltaOf(TimeSpan.FromSeconds(0.9));

			// Act
			loop.ExecuteTicks(TimeSpan.FromSeconds(1));

			// Assert
			func.Received(1).Invoke();
		}

		[Fact]
		public void ExecuteTicks_DeltaOfTwicePerSecond_ExecutesTickTwice()
		{
			// Arrange
			var func = Substitute.For<Action>();
			var loop = Loop
				.ThatCalls(func)
				.WithDeltaOf(TimeSpan.FromSeconds(0.4));

			// Act
			loop.ExecuteTicks(TimeSpan.FromSeconds(1));

			// Assert
			func.Received(2).Invoke();
		}

		[Fact]
		public void ExecuteTicks_NoExplicitDelta_ExecutesTickOnce()
		{
			// Arrange
			var func = Substitute.For<Action>();
			var loop = Loop.ThatCalls(func);

			// Act
			loop.ExecuteTicks(TimeSpan.FromSeconds(1));

			// Assert
			func.Received(1).Invoke();
		}
	}
}