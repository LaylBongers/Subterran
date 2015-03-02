using System.IO;
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
			gameInfo.EngineType = typeof(ValidEngine);

			// Act
			var game = new Game(gameInfo);
			game.Run();

			// Assert
			Assert.True(((ValidEngine)game.Engine).WasRun);
		}
	}
}