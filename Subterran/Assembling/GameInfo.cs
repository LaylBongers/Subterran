using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace Subterran.Assembling
{
	public class GameInfo
	{
		private Type _engineType;

		public string Name { get; set; }
		public Dictionary<string, string> EngineParameters { get; set; }

		public Type EngineType
		{
			get { return _engineType; }
			set
			{
				RunTypeSmoketest(value.FullName, value);
				_engineType = value;
			}
		}

		public static GameInfo FromJson(string json)
		{
			var value = new GameInfo();
			var obj = JObject.Parse(json);

			// Convert simple properties
			value.Name = (string) obj["Name"];

			// Convert the more advanced properties
			LoadType(obj, value);
			LoadParameters(obj, value);

			return value;
		}

		private static void LoadType(JObject obj, GameInfo value)
		{
			// Look for the type we're after for our Engine
			var typeName = (string) obj["EngineType"];
			var types = AppDomain.CurrentDomain.GetAssemblies()
				// Skip dynamic assemblies
				.Where(a => !a.IsDynamic)
				// Look for the specified type in every assembly
				.Select(a => a.GetType(typeName))
				// Skip any assemblies where we didn't find a type
				.Where(t => t != null)
				.ToList();

			RunTypesSmoketest(typeName, types);
			
			// Actually set the type
			value.EngineType = types[0];
		}

		private static void RunTypesSmoketest(string typeName, IReadOnlyList<Type> types)
		{
			// Check for ambiguous or missing type names
			if (!types.Any())
				throw new InvalidOperationException(string.Format("Provided Engine type \"{0}\" could not be found.", typeName));
			if (types.Count != 1)
				throw new InvalidOperationException(string.Format("Provided Engine type \"{0}\" is ambiguous.", typeName));

			RunTypeSmoketest(typeName, types[0]);
		}


		private static void RunTypeSmoketest(string typeName, Type type)
		{
			// Make sure the type we've got follows the requirements
			if (!type.GetConstructors().Any(EngineHelpers.IsValidConstructor))
				throw new InvalidOperationException("Provided Engine type does not have a valid constructor." +
													"Valid constructor has 1 parameter of type Dictionary<string, string>.");

			// Make sure it has a valid entry point as well
			var entryPoints = type.GetMethods().Where(EngineHelpers.IsValidEntryPoint).ToList();
			if (!entryPoints.Any())
				throw new InvalidOperationException(string.Format("Provided Engine type \"{0}\" contains no valid entry point.", typeName));
			if (entryPoints.Count != 1)
				throw new InvalidOperationException(string.Format("Provided Engine type \"{0}\" entry point is ambiguous.", typeName));
		}

		private static void LoadParameters(JObject obj, GameInfo value)
		{
			IDictionary<string, JToken> parameters = (JObject) obj["EngineParameters"];
			value.EngineParameters = parameters.ToDictionary(pair => pair.Key, pair => (string) pair.Value);
		}
	}
}