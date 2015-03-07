﻿using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Subterran.Assembling
{
	public class Game : Disposable
	{
		private readonly Dictionary<string, IAssetSource> _assetSources;

		public Game(GameInfo info)
		{
			_assetSources = new Dictionary<string, IAssetSource>();
		}

		public object Engine { get; set; }

		public void AddAssetSource(string name, IAssetSource source)
		{
			_assetSources.Add(name, source);
		}

		public T GetAsset<T>(string path)
		{
			// TODO: Move assets to a separate Assets class

			// Smoke check
			if (string.IsNullOrEmpty(path))
				throw new ArgumentNullException(path);
			if (path[0] != '@')
				throw new InvalidOperationException("Path is not a valid asset path.");

			// Find the separate source prefix and path of the asset
			var split = path.IndexOf('/');
			var prefix = path.Substring(0, split);
			var relativePath = path.Substring(split, path.Length - split);

			// Check if we've got an asset source for this prefix
			var sourceKey = prefix.TrimStart('@');
			IAssetSource source;
			if (!_assetSources.TryGetValue(sourceKey, out source))
				throw new InvalidOperationException("Asset source \"" + sourceKey + "\" could not be found.");

			// Get data from the source
			var data = source.GetText(relativePath);
			if (data == null)
				throw new InvalidOperationException("Could not retrieve data from asset source.");

			// Parse the data into the requested object
			// TODO: Add custom asset parsing for things that can not just be parsed from json
			return JsonConvert.DeserializeObject<T>(data);
		}

		public void Run()
		{
			throw new NotImplementedException();
		}
	}
}