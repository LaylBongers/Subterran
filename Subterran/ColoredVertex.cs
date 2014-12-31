using System.Runtime.InteropServices;
using OpenTK;

namespace Subterran
{
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct ColoredVertex
	{
		public static readonly ColoredVertex Empty;
		public static readonly int SizeInBytes = Marshal.SizeOf(Empty);

		public Vector3 Position { get; set; }
		public Vector3 Color { get; set; }
	}
}