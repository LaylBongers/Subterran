using System;
using Newtonsoft.Json;

namespace Subterran.Assets
{
	class AssetPathConverter : JsonConverter
	{
		public override bool CanConvert(Type objectType)
		{
			return typeof(AssetPath).IsAssignableFrom(objectType);
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			if(reader == null)
				throw new ArgumentNullException("reader");

			return new AssetPath((string)reader.Value);
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			if (writer == null)
				throw new ArgumentNullException("writer");
			if (value == null)
				throw new ArgumentNullException("value");

			var strongValue = (AssetPath)value;
			writer.WriteValue(strongValue.FullPath);
		}
	}
}
