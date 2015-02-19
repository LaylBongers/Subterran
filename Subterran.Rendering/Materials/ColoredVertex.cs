using System.Runtime.InteropServices;
using OpenTK;

namespace Subterran.Rendering.Materials
{
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct ColoredVertex
	{
		public Vector3 Position;
		public Vector3 Color;
	}
}