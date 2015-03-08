using System;
using Newtonsoft.Json;

namespace Subterran
{
	[JsonConverter(typeof (ServiceConfigConverter))]
	public class ServiceConfig
	{
		public ServiceConfig(string path)
		{
			Path = path;
		}

		public string Path { get; set; }
	}

	internal class ServiceConfigConverter : JsonConverter
	{
		public override bool CanConvert(Type objectType)
		{
			return typeof (ServiceConfig).IsAssignableFrom(objectType);
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			if (reader == null)
				throw new ArgumentNullException("reader");

			return new ServiceConfig((string) reader.Value);
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			if (writer == null)
				throw new ArgumentNullException("writer");
			if (value == null)
				throw new ArgumentNullException("value");

			var strongValue = (ServiceConfig) value;
			writer.WriteValue(strongValue.Path);
		}
	}
}