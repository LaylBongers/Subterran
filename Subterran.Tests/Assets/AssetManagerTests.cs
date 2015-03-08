using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Subterran.Assets;
using Xunit;

namespace Subterran.Tests.Assets
{
	[Trait("Type", "Unit")]
	[Trait("Namespace", "Subterran.Assets")]
	public class AssetManagerTests
	{
		[Fact]
		public void GetAsset_SimpleJson_Deserializes()
		{
			// Arrange
			const string expectedKey = "blah";
			const string expectedValue = "blah";
			var json = new JObject {{expectedKey, expectedValue}};

			var assetManager = new AssetManager();
			assetManager.AddSource("Fake", new FakeAssetSource(json.ToString()));

			// Act
			var value = assetManager.GetAsset<Dictionary<string, string>>("@Fake/Something");

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