using Subterran;

namespace TropicalIsland
{
	internal static class TropicalIsland
	{
		public static StInstance CreateGame()
		{
			return new StInstance(Initialize);
		}

		private static void Initialize(StInstance instance)
		{
			var data = new GameData();

			instance
				.AddLoop(Loop
					.ThatCalls(() => Update(data))
					.WithRateOf(120).PerSecond())
				.AddLoop(Loop
					.ThatCalls(() => Render(data)));

			instance.Uninitialize += (s, e) => Uninitialize();
		}

		private static void Uninitialize()
		{
		}

		private static void Update(GameData data)
		{
		}

		private static void Render(GameData data)
		{
		}

		private class GameData
		{
		}
	}
}