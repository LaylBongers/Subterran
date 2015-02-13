using System.Runtime.InteropServices;
using OpenTK;

namespace Subterran.Rendering.Vertices
{
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct ColoredVertex
	{
		public static readonly ColoredVertex Zero;
		public static readonly int SizeInBytes = Marshal.SizeOf(Zero);

		public Vector3 Position { get; set; }
		public Vector3 Color { get; set; }
	}
}