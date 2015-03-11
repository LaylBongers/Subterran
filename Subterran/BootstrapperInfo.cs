using System;
using Newtonsoft.Json;

namespace Subterran
{
	public class BootstrapperInfo : ICloneable
	{
		[JsonConverter(typeof (TypeConverter))]
		public Type BootstrapperType { get; set; }

		public ConfigPath Configuration { get; set; }

		public object Clone()
		{
			var value = new BootstrapperInfo();

			value.BootstrapperType = BootstrapperType;
			value.Configuration = Configuration;

			return value;
		}
	}
}