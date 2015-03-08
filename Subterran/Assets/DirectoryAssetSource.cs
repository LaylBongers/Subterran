using System.IO;

namespace Subterran.Assets
{
	public class DirectoryAssetSource : IAssetSource
	{
		private readonly DirectoryInfo _directory;

		public DirectoryAssetSource(string path)
		{
			_directory = new DirectoryInfo(path);
		}

		public string GetText(string relativePath)
		{
			var file = _directory.FullName + relativePath;

			if (!File.Exists(file))
				return null;

			return File.ReadAllText(file);
		}
	}
}