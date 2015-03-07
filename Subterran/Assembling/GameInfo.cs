using Newtonsoft.Json.Linq;

namespace Subterran.Assembling
{
	public class GameInfo
	{
		public string Name { get; set; }
		

		public static GameInfo FromJson(string json)
		{
			var value = new GameInfo();
			var obj = JObject.Parse(json);

			// Convert simple properties
			value.Name = (string) obj["Name"];
			
			return value;
		}
	}
}