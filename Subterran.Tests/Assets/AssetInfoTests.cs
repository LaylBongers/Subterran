using Newtonsoft.Json;
using Subterran.Assets;
using Xunit;

namespace Subterran.Tests.Assets
{
	[Trait("Type", "Unit")]
	[Trait("Namespace", "Subterran.Assets")]
	[Trait("Class", "Subterran.Assets.AssetInfoTests")]
	public class AssetInfoTests
	{
		[Fact]
		public void FromString_ValidData_SplitsIntoParts()
		{
			// Act
			var info = AssetInfo.FromString("@Fake/Asset/Path");

			// Assert
			Assert.Equal("Fake", info.Source);
			Assert.Equal("Asset/Path", info.RelativePath);
		}

		[Fact]
		public void ToString_ValidData_CombinesParts()
		{
			// Arrange
			var info = new AssetInfo("Fake", "Asset/Path");

			// Act
			var result = info.ToString();

			// Assert
			Assert.Equal("@Fake/Asset/Path", result);
		}

		[Fact]
		public void Converter_ToJson_Converts()
		{
			// Arrange
			var asset = new AssetInfo("Fake", "Asset/Path");

			// Act
			var json = JsonConvert.SerializeObject(asset);

			// Assert
			Assert.Equal("\"@Fake/Asset/Path\"", json);
		}

		[Fact]
		public void Converter_FromJson_Converts()
		{
			// Arrange
			const string json = "\"@Fake/Asset/Path\"";

			// Act
			var asset = JsonConvert.DeserializeObject<AssetInfo>(json);

			// Assert
			Assert.Equal(asset.Source, "Fake");
			Assert.Equal(asset.RelativePath, "Asset/Path");
		}
	}
}
