using System;
using OpenTK;

namespace Subterran.Rendering.Vertices
{
	public sealed class TexturedVertexRenderer : IVertexRenderer<TexturedVertex>
	{
		public void RenderMeshStreaming(ref Matrix4 matrix, TexturedVertex[] vertices)
		{
			throw new NotImplementedException();
		}

		public void RenderMeshBuffer(ref Matrix4 matrix, int buffer, int length)
		{
			throw new NotImplementedException();
		}
	}
}