using Xunit;

namespace Subterran.Tests
{
	[Trait("Type", "Unit")]
	[Trait("Namespace", "Subterran")]
	[Trait("Class", "Subterran.GameInstance")]
	public class GameInstanceTests
	{
		[Fact(Skip="Working on making coveralls work.")]
		public void Constructor_WithServices_StartsServices()
		{
			// Arrange
			FakeService.Started = false;
			var info = new GameInfo
			{
				Services =
				{
					new ServiceInfo {ServiceType = typeof (FakeService)}
				}
			};

			// Act
			var instance = new GameInstance(info);

			// Assert
			Assert.True(FakeService.Started);

			// Clean
			instance.AwaitStop();
		}

		private class FakeService
		{
			// Don't have to worry about parallelism here because xUnit runs all tests in a class in serial
			public static bool Started { get; set; }

			public void Start()
			{
				Started = true;
			}
		}
	}
}