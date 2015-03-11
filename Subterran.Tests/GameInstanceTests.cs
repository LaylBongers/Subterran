﻿using System;
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
				Bootstrapper = new BootstrapperInfo
				{
					BootstrapperType = typeof(RunningBootstrapper)
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
				Bootstrapper = new BootstrapperInfo
				{
					BootstrapperType = typeof(RunningBootstrapper)
				}
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
				Bootstrapper = new BootstrapperInfo
				{
					BootstrapperType = typeof(RunningBootstrapper)
				}
			};
			var instance = new GameInstance(info);

			// Act & Assert
			Assert.Throws<InvalidOperationException>(() => instance.Run());
		}

		[Fact]
		public void Run_WithBootstrapper_RunsBootstrapper()
		{
			// Arrange
			RunningBootstrapper.Ran = false;
			var info = new GameInfo
			{
				Bootstrapper = new BootstrapperInfo {BootstrapperType = typeof (RunningBootstrapper)}
			};
			var instance = new GameInstance(info);

			// Act
			instance.Run();

			// Assert
			Assert.True(RunningBootstrapper.Ran);
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

		private sealed class RunningBootstrapper : IBootstrapper
		{
			public static bool Ran { get; set; }

			public void Run()
			{
				Ran = true;
			}
		}
	}
}