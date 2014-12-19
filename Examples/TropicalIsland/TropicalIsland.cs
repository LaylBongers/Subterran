using System.Diagnostics;
using Subterran.Basic;

namespace TropicalIsland
{
	internal sealed class TropicalIsland : BasicSubterranGame
	{
		public TropicalIsland()
			:base("Tropical Island")
		{
			Trace.TraceInformation("Initialize()");
		}

		protected override void Uninitialize()
		{
			Trace.TraceInformation("Uninitialize()");

			base.Uninitialize();
		}

		protected override void Update()
		{
			base.Update();

			// Update stuff here
		}

		protected override void Render()
		{
			base.Render();

			Renderer.RenderTest();

			Window.SwapBuffers();
		}
	}
}