using System;
using Newtonsoft.Json;

namespace Subterran
{
	public class ServiceInfo : ICloneable
	{
		[JsonConverter(typeof(TypeConverter))]
		public Type ServiceType { get; set; }
		public ConfigPath Configuration { get; set; }

		public object Clone()
		{
			var value = new ServiceInfo();

			value.ServiceType = ServiceType;
			value.Configuration = Configuration;

			return value;
		}
	}
}