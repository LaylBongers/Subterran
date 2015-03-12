using Subterran.Assets;

namespace Subterran.WorldState
{
	public class StandardWorldStateService : IWorldStateService
	{
		private IAssetService _assets;

		public StandardWorldStateService(IAssetService assets)
		{
			_assets = assets;
		}

		public void LoadDefault()
		{
		}

		public void Load(AssetPath path)
		{
		}
	}
}