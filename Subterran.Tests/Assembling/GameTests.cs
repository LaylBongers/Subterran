using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Linq;
using Subterran.Assembling;
using Xunit;

namespace Subterran.Tests.Assembling
{
	[Trait("Type", "Unit")]
	[Trait("Namespace", "Subterran.Assembling")]
	public class GameTests
	{
		[Fact]
		public void Constructor_ValidData_CreatesEngineOfGivenType()
		{
			// Arrange
			var expected = typeof (ValidEngine);
			var gameInfo = GameInfo.FromJson(File.ReadAllText("./Assembling/MinimalValidGameInfo.json"));
			gameInfo.EngineType = expected;

			// Act
			var game = new Game(gameInfo);

			// Assert
			Assert.Equal(expected, game.Engine.GetType());
		}

		[Fact]
		public void Run_ValidEngine_RunsEngine()
		{
			// Arrange
			var gameInfo = GameInfo.FromJson(File.ReadAllText("./Assembling/MinimalValidGameInfo.json"));
			gameInfo.EngineType = typeof (ValidEngine);

			// Act
			var game = new Game(gameInfo);
			game.Run();

			// Assert
			Assert.True(((ValidEngine) game.Engine).WasRun);
		}

		[Fact]
		public void GetAsset_SimpleJson_Deserializes()
		{
			// Arrange
			const string expectedKey = "blah";
			const string expectedValue = "blah";
			var json = new JObject {{expectedKey, expectedValue}};

			var gameInfo = GameInfo.FromJson(File.ReadAllText("./Assembling/MinimalValidGameInfo.json"));
			var game = new Game(gameInfo);
			game.AddAssetSource("fake", new FakeAssetSource(json.ToString()));

			// Act
			var value = game.GetAsset<Dictionary<string, string>>("@fake/something");

			// Assert
			Assert.True(value.ContainsKey(expectedKey));
			Assert.Equal(expectedValue, value[expectedKey]);
		}

		private class FakeAssetSource : IAssetSource
		{
			private readonly string _text;

			public FakeAssetSource(string text)
			{
				_text = text;
			}

			public string GetText(string relativePath)
			{
				return _text;
			}
		}
	}
}