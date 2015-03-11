namespace Subterran.Assets
{
	public interface IAssetService
	{
		void AddSource(string name, IAssetSource source);
		T GetAsset<T>(string path);
		T GetAsset<T>(AssetPath path);
	}
}