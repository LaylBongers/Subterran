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

			var json = JObject.Parse(File.ReadAllText("./Assembling/MinimalValidGameInfo.json"));
			json["Name"] = expectedName;

			// Act
			var info = GameInfo.FromJson(json.ToString());

			// Assert
			Assert.Equal(expectedName, info.Name);
		}
	}
}