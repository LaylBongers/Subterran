using OpenTK;

namespace Subterran.Rendering
{
	public interface IVertexRenderer
	{
	}

	public interface IVertexRenderer<in TVertex> : IVertexRenderer
	{
		void RenderMeshStreaming(ref Matrix4 matrix, TVertex[] vertices);
		void RenderMeshBuffer(ref Matrix4 matrix, int buffer, int length);
	}
}