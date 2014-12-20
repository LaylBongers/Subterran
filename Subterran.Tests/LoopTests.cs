﻿using System;
using NSubstitute;
using Xunit;

namespace Subterran.Tests
{
	public class LoopTests
	{
		[Fact]
		public void WithDeltaOf_OneSecond_ExecutesTickOnce()
		{
			// Arrange
			var func = Substitute.For<Action>();
			var loop = Loop.ThatCalls(_ => func());

			// Act
			loop = loop.WithDeltaOf(TimeSpan.FromSeconds(0.9999));

			// Assert
			loop.ExecuteTicks(TimeSpan.FromSeconds(1));
			func.Received(1).Invoke();
		}

		[Fact]
		public void WithDeltaOf_HalfSecond_ExecutesTickTwice()
		{
			// Arrange
			var func = Substitute.For<Action>();
			var loop = Loop.ThatCalls(_ => func());

			// Act
			loop = loop.WithDeltaOf(TimeSpan.FromSeconds(0.4999));

			// Assert
			loop.ExecuteTicks(TimeSpan.FromSeconds(1));
			func.Received(2).Invoke();
		}

		[Fact]
		public void ExecuteTicks_NoExplicitDelta_ExecutesTickOnce()
		{
			// Arrange
			var func = Substitute.For<Action>();
			var loop = Loop.ThatCalls(_ => func());

			// Act
			loop.ExecuteTicks(TimeSpan.FromSeconds(1));

			// Assert
			func.Received(1).Invoke();
		}

		[Fact]
		public void WithRateOf_TwoPerSecond_ExecutesTickTwice()
		{
			// Arrange
			var func = Substitute.For<Action>();
			var loop = Loop.ThatCalls(_ => func());

			// Act
			loop = loop.WithRateOf(2).PerSecond();

			// Assert
			loop.ExecuteTicks(TimeSpan.FromSeconds(1));
			func.Received(2).Invoke();
		}
	}
}