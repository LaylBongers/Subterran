using Newtonsoft.Json.Linq;
using Xunit;

namespace Subterran.Tests
{
	[Trait("Type", "Unit")]
	[Trait("Namespace", "Subterran")]
	[Trait("Class", "Subterran.GameInfo")]
	public class GameInfoTests
	{
		[Fact]
		public void FromJson_SimpleData_Deserializes()
		{
			// Arrange
			const string expectedName = "The Best Game Ever";
			var json = new JObject
			{
				["Name"] = expectedName,
				["Services"] = new JArray(),
				["Bootstrapper"] = new JObject
				{
					["BootstrapperType"] = typeof (FakeBootstrapper).FullName,
					["Configuration"] = "Bootstrapper.json"
				}
			};

			// Act
			var info = GameInfo.FromJson(json.ToString());

			// Assert
			Assert.Equal(expectedName, info.Name);
			Assert.NotNull(info.Bootstrapper);
			Assert.Equal(typeof (FakeBootstrapper), info.Bootstrapper.BootstrapperType);
			Assert.Equal("Bootstrapper.json", info.Bootstrapper.Configuration.Path);
		}

		[Fact]
		public void FromJson_WithService_Deserializes()
		{
			// Arrange
			var json = new JObject
			{
				["Name"] = "Irrelevant",
				["Bootstrapper"] = new JObject
				{
					["BootstrapperType"] = typeof (FakeBootstrapper).FullName,
					["Configuration"] = "Bootstrapper.json"
				},
				["Services"] = new JArray
				{
					new JObject
					{
						["ServiceType"] = "Subterran.Tests.GameInfoTests+FakeService",
						["Configuration"] = "FakeFile.json"
					}
				}
			};

			// Act
			var info = GameInfo.FromJson(json.ToString());
			var services = info.Services;

			// Assert
			Assert.Equal(1, services.Count);
			var service = services[0];
			Assert.Equal(typeof (FakeService), service.ServiceType);
			Assert.Equal("FakeFile.json", service.Configuration.Path);
		}

		private sealed class FakeService
		{
		}

		private sealed class FakeBootstrapper
		{
		}
	}
}