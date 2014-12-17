using System.Diagnostics;
using Subterran.Basic;

namespace TropicalIsland
{
	internal sealed class TropicalIsland : BasicSubterranGame
	{
		public TropicalIsland()
		{
			Trace.TraceInformation("Initialize()");
		}

		protected override void Uninitialize()
		{
			Trace.TraceInformation("Uninitialize()");
		}

		protected override void Update()
		{
			Trace.TraceInformation("Update()");
		}

		protected override void Render()
		{
			Trace.TraceInformation("Render()");
		}
	}
}