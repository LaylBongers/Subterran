using System;

namespace Subterran
{
	public class ServiceInfo : ICloneable
	{
		public Type ServiceType { get; set; }
		public ServiceConfig Configuration { get; set; }

		public object Clone()
		{
			var value = new ServiceInfo();

			value.ServiceType = ServiceType;
			value.Configuration = Configuration;

			return value;
		}
	}
}