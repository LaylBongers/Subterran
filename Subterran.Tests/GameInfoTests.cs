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
				["Services"] = new JArray()
			};

			// Act
			var info = GameInfo.FromJson(json.ToString());

			// Assert
			Assert.Equal(expectedName, info.Name);
		}

		[Fact]
		public void FromJson_WithServices_Deserializes()
		{
			// Arrange
			var json = new JObject
			{
				["Name"] = "Irrelevant",
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
			Assert.Equal(typeof(FakeService), service.ServiceType);
			Assert.Equal("FakeFile.json", service.Configuration);
		}

		private class FakeService
		{
		}
	}
}