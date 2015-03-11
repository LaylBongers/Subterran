using System;
using Newtonsoft.Json;

namespace Subterran
{
	[JsonConverter(typeof (ConfigPathConverter))]
	public class ConfigPath
	{
		public ConfigPath(string path)
		{
			Path = path;
		}

		public string Path { get; }
	}

	internal class ConfigPathConverter : JsonConverter
	{
		public override bool CanConvert(Type objectType)
		{
			return typeof (ConfigPath).IsAssignableFrom(objectType);
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			if (reader == null)
				throw new ArgumentNullException("reader");

			return new ConfigPath((string) reader.Value);
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			if (writer == null)
				throw new ArgumentNullException("writer");
			if (value == null)
				throw new ArgumentNullException("value");

			var strongValue = (ConfigPath) value;
			writer.WriteValue(strongValue.Path);
		}
	}
}