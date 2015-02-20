using System.Runtime.InteropServices;
using OpenTK;

namespace Subterran.Toolbox.Materials
{
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct ColoredVertex
	{
		public Vector3 Position;
		public Vector3 Color;
	}
}