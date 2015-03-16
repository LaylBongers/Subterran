using NSubstitute;
using Subterran.Assets;
using Subterran.WorldState;
using Xunit;

namespace Subterran.Tests.WorldState
{
	[Trait("Type", "Unit")]
	[Trait("Namespace", "Subterran.WorldState")]
	[Trait("Class", "Subterran.WorldState.StandardWorldStateService")]
	public class StandardWorldStateServiceTests
	{
		[Fact]
		public void LoadDefault_WithConfig_LoadsDefaultWorld()
		{
			// Arrange
			var assetInfo = AssetInfo.FromString("@Assets/TestScene.json");
			const string rootEntityName = "Test World";

			var sceneInfo = new SceneInfo
			{
				Root = new EntityInfo
				{
					Name = rootEntityName
				}
			};

			var assets = Substitute.For<IAssetService>();
			assets.GetAsset<SceneInfo>(assetInfo).Returns(sceneInfo);

			var world = new StandardWorldStateService(new ServiceInfo {ConfigString = assetInfo.ToString()}, assets);

			// Act
			world.LoadDefault();

			// Assert
			Assert.Equal(rootEntityName, world.World.Name);
		}

		public void Load_WithEntityInScene_LoadsEntity()
		{
			// Arrange
			var assetInfo = AssetInfo.FromString("@Assets/TestScene.json");
			const string rootEntityName = "Test World";
			const string childEntityName = "Test Entity";

			var sceneInfo = new SceneInfo
			{
				Root = new EntityInfo
				{
					Name = rootEntityName,
					Children =
					{
						new EntityInfo {Name = childEntityName}
					}
				}
			};

			var assets = Substitute.For<IAssetService>();
			assets.GetAsset<SceneInfo>(assetInfo).Returns(sceneInfo);

			var world = new StandardWorldStateService(new ServiceInfo {ConfigString = assetInfo.ToString()}, assets);

			// Act
			world.Load(assetInfo);

			// Assert
			Assert.Equal(rootEntityName, world.World.Name);
			Assert.Equal(1, world.World.Children.Count);
			Assert.Equal(childEntityName, world.World.Children[0].Name);
		}
	}
}