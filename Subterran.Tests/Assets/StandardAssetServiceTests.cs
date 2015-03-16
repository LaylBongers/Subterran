using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Linq;
using Subterran.Assets;
using Xunit;

namespace Subterran.Tests.Assets
{
	[Trait("Type", "Unit")]
	[Trait("Namespace", "Subterran.Assets")]
	[Trait("Class", "Subterran.Assets.StandardAssetService")]
	public class StandardAssetServiceTests
	{
		[Fact]
		public void GetAsset_SimpleJson_Deserializes()
		{
			// Arrange
			const string expectedKey = "blah";
			const string expectedValue = "blah";
			var json = new JObject {{expectedKey, expectedValue}};

			var assetManager = new StandardAssetService(new ServiceInfo());
			assetManager.AddSource("Fake", new FakeAssetSource(json.ToString()));

			// Act
			var value = assetManager.GetAsset<Dictionary<string, string>>(AssetInfo.FromString("@Fake/Something"));

			// Assert
			Assert.True(value.ContainsKey(expectedKey));
			Assert.Equal(expectedValue, value[expectedKey]);
		}

		[Fact]
		public void Constructor_FileInConfig_LoadsFromFile()
		{
			// Act
			const string json = "{\"Services\":[]}";
			var file = Path.GetRandomFileName();
			File.WriteAllText(file, json);
			var assets = new StandardAssetService(new ServiceInfo {ConfigString = file});

			// Assert
			//assets.HasSource("");
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