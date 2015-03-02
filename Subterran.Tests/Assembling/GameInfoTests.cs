using System;
using System.IO;
using Newtonsoft.Json.Linq;
using Subterran.Assembling;
using Xunit;

namespace Subterran.Tests.Assembling
{
	[Trait("Type", "Unit")]
	[Trait("Namespace", "Subterran.Assembling")]
	public class GameInfoTests
	{
		[Fact]
		public void FromJson_SimpleData_Deserializes()
		{
			// Arrange
			const string expectedName = "The Best Game Ever";
			var expectedType = typeof (ValidEngine);

			var json = JObject.Parse(File.ReadAllText("./Assembling/MinimalValidGameInfo.json"));
			json["Name"] = expectedName;
			json["EngineType"] = expectedType.FullName;

			// Act
			var info = GameInfo.FromJson(json.ToString());

			// Assert
			Assert.Equal(expectedName, info.Name);
			Assert.Equal(expectedType, info.EngineType);
		}

		[Fact]
		public void FromJson_EngineParameters_Deserializes()
		{
			// Arrange
			const string expectedA = "blah";
			const string expectedB = "bluh";
			var expected = new JObject
			{
				{"expectedA", expectedA},
				{"expectedB", expectedB}
			};

			var json = JObject.Parse(File.ReadAllText("./Assembling/MinimalValidGameInfo.json"));
			json["EngineParameters"] = expected;

			// Act
			var info = GameInfo.FromJson(json.ToString());

			// Assert
			Assert.Equal(expected.Count, info.EngineParameters.Count);
			Assert.Equal(expectedA, info.EngineParameters["expectedA"]);
			Assert.Equal(expectedB, info.EngineParameters["expectedB"]);
		}

		[Fact]
		public void FromJson_NonExistentEngineType_ThrowsException()
		{
			// Arrange
			const string fakeEngine = "Subterran.Tests.Assembling.GameInfoTests+FakeEngine";

			var json = JObject.Parse(File.ReadAllText("./Assembling/MinimalValidGameInfo.json"));
			json["EngineType"] = fakeEngine;

			// Act & Assert
			Assert.Throws<InvalidOperationException>(() => GameInfo.FromJson(json.ToString()));
		}

		[Fact]
		public void FromJson_InvalidEngineType_ThrowsException()
		{
			// Arrange
			var json = JObject.Parse(File.ReadAllText("./Assembling/MinimalValidGameInfo.json"));
			json["EngineType"] = typeof (InvalidEngine).FullName;

			// Act & Assert
			Assert.Throws<InvalidOperationException>(() => GameInfo.FromJson(json.ToString()));
		}

		[Fact]
		public void SetEngineType_InvalidEngineType_ThrowsException()
		{
			// Arrange
			var info = new GameInfo();

			// Act & Assert
			Assert.Throws<InvalidOperationException>(() => info.EngineType = typeof (InvalidEngine));
		}
	}
}