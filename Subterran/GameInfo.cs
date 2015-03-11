using System;
using System.Collections.ObjectModel;
using System.Linq;
using Newtonsoft.Json;

namespace Subterran
{
	/// <summary>
	///     Represents info about a Subterran game.
	/// </summary>
	public class GameInfo : ICloneable
	{
		public string Name { get; set; }
		public Collection<ServiceInfo> Services { get; } = new Collection<ServiceInfo>();

		[JsonConverter(typeof (TypeConverter))]
		public Type GameLoopType { get; set; }

		public object Clone()
		{
			var value = new GameInfo();

			value.Name = Name; // strings are immutable, no need to clone
			Services.Select(s => (ServiceInfo) s.Clone()).AddTo(value.Services);
			value.GameLoopType = GameLoopType;

			return value;
		}

		public static GameInfo FromJson(string json)
		{
			return JsonConvert.DeserializeObject<GameInfo>(json);
		}
	}
}