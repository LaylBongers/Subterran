using System;
using System.Linq;

namespace Subterran.Assembling
{
	public class Game : Disposable
	{
		private readonly Type _engineType;

		public Game(GameInfo info)
		{
			_engineType = info.EngineType;
			Engine = CreateEngine(info);
		}

		public object Engine { get; set; }

		public void Run()
		{
			// Find the entry point of the engine
			var entryPoint = _engineType.GetMethods().First(EngineHelpers.IsValidEntryPoint);
			entryPoint.Invoke(Engine, new object[]{});
		}

		private static object CreateEngine(GameInfo info)
		{
			var constructor = info.EngineType.GetConstructors().First(EngineHelpers.IsValidConstructor);
			return constructor.Invoke(new object[] {info.EngineParameters});
		}
	}
}