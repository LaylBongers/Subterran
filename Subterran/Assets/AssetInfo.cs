using System;
using System.Text;
using Newtonsoft.Json;

namespace Subterran.Assets
{
	[JsonConverter(typeof (AssetPathConverter))]
	public class AssetInfo : IEquatable<AssetInfo>
	{
		public AssetInfo(string source, string relativePath)
		{
			if (string.IsNullOrEmpty(source))
				throw new ArgumentNullException("source");
			if (string.IsNullOrEmpty(relativePath))
				throw new ArgumentNullException("relativePath");

			Source = source;
			RelativePath = relativePath;
		}

		public string Source { get; }
		public string RelativePath { get; }

		public bool Equals(AssetInfo other)
		{
			if (ReferenceEquals(null, other)) return false;
			if (ReferenceEquals(this, other)) return true;

			return string.Equals(Source, other.Source) && string.Equals(RelativePath, other.RelativePath);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;

			if (obj.GetType() != GetType()) return false;

			return Equals((AssetInfo) obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return (Source.GetHashCode()*397) ^ RelativePath.GetHashCode();
			}
		}

		public static bool operator ==(AssetInfo left, AssetInfo right)
		{
			return Equals(left, right);
		}

		public static bool operator !=(AssetInfo left, AssetInfo right)
		{
			return !Equals(left, right);
		}

		public override string ToString()
		{
			var builder = new StringBuilder();
			builder.Append("@");
			builder.Append(Source);
			builder.Append("/");
			builder.Append(RelativePath);
			return builder.ToString();
		}

		public static AssetInfo FromString(string fullPath)
		{
			if (string.IsNullOrEmpty(fullPath))
				throw new ArgumentNullException("fullPath");
			if (fullPath[0] != '@')
				throw new InvalidOperationException("Path is not a valid asset path.");

			// If we don't have a first splitting slash it's not a valid path
			var split = fullPath.IndexOf('/');
			if (split == -1)
				throw new InvalidOperationException("Path is not a valid asset path.");

			var source = fullPath.Substring(1, split - 1);
			var relative = fullPath.Substring(split + 1, fullPath.Length - split - 1);

			return new AssetInfo(source, relative);
		}
	}

	internal class AssetPathConverter : JsonConverter
	{
		public override bool CanConvert(Type objectType)
		{
			return typeof (AssetInfo).IsAssignableFrom(objectType);
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			if (reader == null)
				throw new ArgumentNullException("reader");

			return AssetInfo.FromString((string) reader.Value);
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			if (writer == null)
				throw new ArgumentNullException("writer");
			if (value == null)
				throw new ArgumentNullException("value");

			var strongValue = (AssetInfo) value;
			writer.WriteValue(strongValue.ToString());
		}
	}
}