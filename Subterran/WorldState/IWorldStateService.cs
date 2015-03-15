using Subterran.Assets;

namespace Subterran.WorldState
{
	public interface IWorldStateService
	{
		void LoadDefault();
		void Load(AssetInfo path);
	}
}