using System;
using Xunit;

namespace Subterran.Tests
{
	public class LoopManagerTests
	{
		[Fact]
		public void Run_NoLoops_ThrowsException()
		{
			// Arrange
			var manager = new LoopManager();

			// Act/Assert
			Assert.Throws<InvalidOperationException>(() => manager.Run());
		}

		[Fact]
		public void Run_OneLoop_UpdatesLoop()
		{
			// Arrange
			var manager = new LoopManager();
			var called = false;
			manager.Loops.Add(new GameLoopTimer(_ =>
			{
				// By putting this check in front, it runs twice
				if (called)
					manager.Stop();

				called = true;
			}));

			// Act
			manager.Run();

			// Assert
			Assert.True(called);
		}
	}
}