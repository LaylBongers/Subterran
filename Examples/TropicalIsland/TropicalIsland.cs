using System.Diagnostics;
using Subterran.Basic;

namespace TropicalIsland
{
	internal sealed class TropicalIsland : BasicSubterranGame
	{
		public TropicalIsland()
			: base("Tropical Island")
		{
			Trace.TraceInformation("Initializing...");
		}

		protected override void Dispose(bool managed)
		{
			if (managed)
			{
				Trace.TraceInformation("Disposing...");
			}

			base.Dispose(managed);
		}
	}
}