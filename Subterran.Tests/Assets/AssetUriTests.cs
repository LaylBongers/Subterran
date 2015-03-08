using Subterran.Assets;
using Xunit;

namespace Subterran.Tests.Assets
{
	[Trait("Type", "Unit")]
	[Trait("Namespace", "Subterran.Assets")]
	public class AssetUriTests
	{
		[Fact]
		public void Constructor_ValidData_SplitsIntoParts()
		{
			// Arrange

			// Act
			var uri = new AssetPath("@Fake/Asset/Path");

			// Assert
			Assert.Equal("Fake", uri.Source);
			Assert.Equal("Asset/Path", uri.RelativePath.TrimStart('/'));
		}
	}
}
