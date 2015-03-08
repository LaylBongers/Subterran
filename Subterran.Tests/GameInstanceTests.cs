using System;
using Xunit;

namespace Subterran.Tests
{
	[Trait("Type", "Unit")]
	[Trait("Namespace", "Subterran")]
	[Trait("Class", "Subterran.GameInstance")]
	public class GameInstanceTests
	{
		[Fact]
		public void Run_WithServices_StartsAndStopsServices()
		{
			// Arrange
			StartingService.Started = false;
			var info = new GameInfo
			{
				Services =
				{
					new ServiceInfo {ServiceType = typeof (StartingService)},
					new ServiceInfo {ServiceType = typeof (StoppingService)}
				}
			};

			// Act
			var instance = new GameInstance(info);
			instance.Run();

			// Assert
			Assert.True(StartingService.Started, "Service was not started.");
			Assert.True(StoppingService.Stopped, "Service was not stopped.");
		}

		private sealed class StartingService
		{
			// Don't have to worry about parallelism here because xUnit runs all tests in a class in serial
			public static bool Started { get; set; }

			public StartingService()
			{
				Started = true;
			}
		}

		private sealed class StoppingService : IDisposable
		{
			// Don't have to worry about parallelism here because xUnit runs all tests in a class in serial
			public static bool Stopped { get; set; }
			
			public void Dispose()
			{
				Stopped = true;
			}
		}
	}
}