using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace Subterran
{
	/// <summary>
	///     Represents info about a Subterran game.
	/// </summary>
	public class GameInfo : ICloneable
	{
		public string Name { get; set; }
		public Collection<ServiceInfo> Services { get; } = new Collection<ServiceInfo>();

		public object Clone()
		{
			var value = new GameInfo();

			value.Name = Name; // strings are immutable, no need to clone
			Services.Select(s => (ServiceInfo) s.Clone()).AddTo(value.Services);

			return value;
		}

		#region Json Parsing

		public static GameInfo FromJson(string json)
		{
			var value = new GameInfo();
			var obj = JObject.Parse(json);

			// Convert simple properties
			value.Name = obj["Name"].ToObject<string>();

			// Convert all the services
			ReadServices(obj, value);

			return value;
		}

		private static void ReadServices(JObject obj, GameInfo value)
		{
			var services = obj["Services"].ToObject<JArray>();
			foreach (var item in services)
			{
				var service = new ServiceInfo();

				service.ServiceType = GetTypeFromName(item["ServiceType"].ToObject<string>());
				service.Configuration = item["Configuration"].ToObject<ServiceConfig>();

				value.Services.Add(service);
			}
		}

		private static Type GetTypeFromName(string typeName)
		{
			var types = AppDomain.CurrentDomain
				.GetAssemblies()
				.Select(a => a.GetType(typeName))
				.Where(t => t != null)
				.ToList();

			if (types.Count == 0)
				ThrowTypeError(ExceptionMessages.GameInfo_TypeNotFound, typeName);

			if (types.Count > 1)
				ThrowTypeError(ExceptionMessages.GameInfo_TypeAmbiguous, typeName);
			
			return types[0];
		}

		private static void ThrowTypeError(string message, string typeName)
		{
			throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, message, typeName));
		}

		#endregion
	}
}