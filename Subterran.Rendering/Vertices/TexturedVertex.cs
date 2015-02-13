using System.Runtime.InteropServices;
using OpenTK;

namespace Subterran.Rendering.Vertices
{
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public class TexturedVertex
	{
		public Vector3 Position { get; set; }
		public Vector2 TexCoord { get; set; }
	}
}