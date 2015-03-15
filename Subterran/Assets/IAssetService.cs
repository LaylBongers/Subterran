namespace Subterran.Assets
{
	public interface IAssetService
	{
		void AddSource(string name, IAssetSource source);
		T GetAsset<T>(AssetInfo path);
	}
}