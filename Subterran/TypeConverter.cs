using System;
using Newtonsoft.Json;

namespace Subterran
{
	internal class TypeConverter : JsonConverter
	{
		public override bool CanConvert(Type objectType)
		{
			return typeof (Type).IsAssignableFrom(objectType);
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			if (reader == null)
				throw new ArgumentNullException("reader");
			
			return StReflection.GetTypeFromName((string)reader.Value);
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			if (writer == null)
				throw new ArgumentNullException("writer");
			if (value == null)
				throw new ArgumentNullException("value");

			var strongValue = (Type)value;
			writer.WriteValue(strongValue.FullName);
		}
	}
}
