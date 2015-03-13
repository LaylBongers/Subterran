using System;
using Newtonsoft.Json;

namespace Subterran
{
	public class ServiceInfo : ICloneable
	{
		[JsonConverter(typeof(TypeConverter))]
		public Type ServiceType { get; set; }

		public string ConfigString { get; set; }

		public object Clone()
		{
			var value = new ServiceInfo();

			value.ServiceType = ServiceType;
			value.ConfigString = ConfigString;

			return value;
		}
	}
}