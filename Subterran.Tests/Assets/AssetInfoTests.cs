using Subterran.Assets;
using Xunit;

namespace Subterran.Tests.Assets
{
	[Trait("Type", "Unit")]
	[Trait("Namespace", "Subterran.Assets")]
	[Trait("Class", "Subterran.Assets.AssetUri")]
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
	}
}
