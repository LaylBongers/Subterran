using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Subterran.Assets
{
	public sealed class StandardAssetService : IAssetService
	{
		private readonly Dictionary<string, IAssetSource> _assetSources = new Dictionary<string, IAssetSource>();
		
		public void AddSource(string name, IAssetSource source)
		{
			_assetSources.Add(name, source);
		}

		public T GetAsset<T>(string path)
		{
			return GetAsset<T>(new AssetPath(path));
		}

		public T GetAsset<T>(AssetPath path)
		{
			if (path == null)
				throw new ArgumentNullException("path");

			// Check if we've got an asset source for this prefix
			IAssetSource source;
			if (!_assetSources.TryGetValue(path.Source, out source))
				throw new InvalidOperationException("Asset source \"" + path.Source + "\" could not be found.");

			// Get data from the source
			var data = source.GetText(path.RelativePath);
			if (data == null)
				throw new InvalidOperationException("Could not retrieve data from asset source.");

			// Parse the data into the requested object
			// TODO: Add custom asset parsing for things that can not just be parsed from json
			return JsonConvert.DeserializeObject<T>(data);
		}
	}
}