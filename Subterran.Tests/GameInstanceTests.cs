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
			StoppingService.Stopped = false;
			var info = new GameInfo
			{
				Services =
				{
					new ServiceInfo {ServiceType = typeof (StartingService)},
					new ServiceInfo {ServiceType = typeof (StoppingService)}
				}
			};

			var instance = new GameInstance(info);

			// Act
			instance.Run();

			// Assert
			Assert.True(StartingService.Started, "Service was not started.");
			Assert.True(StoppingService.Stopped, "Service was not stopped.");
		}

		[Fact]
		public void Run_WithServices_GetsDependency()
		{
			// Arrange
			DependentService.GotService = false;
			var info = new GameInfo
			{
				Services =
				{
					new ServiceInfo {ServiceType = typeof (DependentService)},
					new ServiceInfo {ServiceType = typeof (DependencyService)}
				}
			};
			var instance = new GameInstance(info);

			// Act
			instance.Run();

			// Assert
			Assert.True(DependentService.GotService);
		}

		private sealed class StartingService
		{
			public StartingService()
			{
				Started = true;
			}

			// Don't have to worry about parallelism here because xUnit runs all tests in a class in serial
			public static bool Started { get; set; }
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

		private sealed class DependentService
		{
			public DependentService(DependencyService service)
			{
				if (service != null)
					GotService = true;
			}

			public static bool GotService { get; set; }
		}

		private sealed class DependencyService
		{
		}
	}
}