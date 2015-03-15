using Subterran.Assets;

namespace Subterran.WorldState
{
	public class StandardWorldStateService : IWorldStateService
	{
		private readonly IAssetService _assets;
		private readonly AssetInfo _defaultScene;

		public StandardWorldStateService(ServiceInfo info, IAssetService assets)
		{
			StContract.ArgumentNotNull(info, "info");
			StContract.ArgumentNotNull(assets, "assets");

			_assets = assets;

			_defaultScene = AssetInfo.FromString(info.ConfigString);
        }

		public Entity World { get; set; }

		public void LoadDefault()
		{
			Load(_defaultScene);
		}

		public void Load(AssetInfo path)
		{
			var sceneInfo = _assets.GetAsset<SceneInfo>(_defaultScene);
			World = sceneInfo.ToWorld();
		}
	}
}