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
				},
				GameLoopType = typeof (RunningGameLoop)
			};

			var instance = new GameInstance(info);

			// Act
			instance.Run();

			// Assert
			Assert.True(StartingService.Started, "Service was not started.");
			Assert.True(StoppingService.Stopped, "Service was not stopped.");
		}

		[Fact]
		public void Run_WithServiceThatNeedsGivenDependency_GetsDependency()
		{
			// Arrange
			DependentService.GotService = false;
			var info = new GameInfo
			{
				Services =
				{
					new ServiceInfo {ServiceType = typeof (DependentService)},
					new ServiceInfo {ServiceType = typeof (DependencyService)}
				},
				GameLoopType = typeof (RunningGameLoop)
			};
			var instance = new GameInstance(info);

			// Act
			instance.Run();

			// Assert
			Assert.True(DependentService.GotService);
		}

		[Fact]
		public void Run_WithServiceThatNeedsMissingDependency_ThrowsException()
		{
			// Arrange
			var info = new GameInfo
			{
				Services =
				{
					new ServiceInfo {ServiceType = typeof (DependentService)}
				},
				GameLoopType = typeof (RunningGameLoop)
			};
			var instance = new GameInstance(info);

			// Act & Assert
			Assert.Throws<InvalidOperationException>(() => instance.Run());
		}

		[Fact]
		public void Run_WithGameLoop_RunsGameLoop()
		{
			// Arrange
			RunningGameLoop.Ran = false;
			var info = new GameInfo
			{
				GameLoopType = typeof (RunningGameLoop)
			};
			var instance = new GameInstance(info);

			// Act
			instance.Run();

			// Assert
			Assert.True(RunningGameLoop.Ran);
		}

		[Fact]
		public void Run_WithServiceThatNeedsServiceInfo_GetsOwnInfo()
		{
			// Arrange
			InformedService.ReceivedInfo = null;
			var info = new GameInfo
			{
				Services =
				{
					new ServiceInfo {ServiceType = typeof (InformedService), ConfigString = "Maaaagic!"}
				},
				GameLoopType = typeof (RunningGameLoop)
			};
			var instance = new GameInstance(info);

			// Act
			instance.Run();

			// Assert
			Assert.NotNull(InformedService.ReceivedInfo);
			Assert.Equal(InformedService.ReceivedInfo?.ServiceType, typeof (InformedService));
			Assert.Equal(InformedService.ReceivedInfo?.ConfigString, "Maaaagic!");
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

		private interface IDependencyService
		{
		}

		private sealed class DependentService
		{
			public DependentService(IDependencyService service)
			{
				if (service != null)
					GotService = true;
			}

			public static bool GotService { get; set; }
		}

		private sealed class DependencyService : IDependencyService
		{
		}

		private sealed class RunningGameLoop : IGameLoop
		{
			public static bool Ran { get; set; }
			public event EventHandler Stopped = (s, e) => { };

			public void Run()
			{
				Ran = true;
			}

			public void StopRunning()
			{
			}
		}

		private sealed class InformedService
		{
			public InformedService(ServiceInfo info)
			{
				ReceivedInfo = info;
			}

			public static ServiceInfo ReceivedInfo { get; set; }
		}
	}
}