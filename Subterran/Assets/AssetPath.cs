using System;
using Newtonsoft.Json;

namespace Subterran.Assets
{
	[JsonConverter(typeof (AssetPathConverter))]
	public class AssetPath
	{
		public AssetPath(string fullPath)
		{
			if (string.IsNullOrEmpty(fullPath))
				throw new ArgumentNullException("fullPath");
			if (fullPath[0] != '@')
				throw new InvalidOperationException("Path is not a valid asset path.");

			FullPath = fullPath;

			// If we don't have a first splitting slash it's not a valid path
			var split = fullPath.IndexOf('/');
			if (split == -1)
				throw new InvalidOperationException("Path is not a valid asset path.");

			Source = fullPath.Substring(1, split - 1);
			RelativePath = fullPath.Substring(split, fullPath.Length - split);
		}

		public string FullPath { get; }
		public string Source { get; }
		public string RelativePath { get; }
	}

	internal class AssetPathConverter : JsonConverter
	{
		public override bool CanConvert(Type objectType)
		{
			return typeof (AssetPath).IsAssignableFrom(objectType);
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			if (reader == null)
				throw new ArgumentNullException("reader");

			return new AssetPath((string) reader.Value);
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			if (writer == null)
				throw new ArgumentNullException("writer");
			if (value == null)
				throw new ArgumentNullException("value");

			var strongValue = (AssetPath) value;
			writer.WriteValue(strongValue.FullPath);
		}
	}
}