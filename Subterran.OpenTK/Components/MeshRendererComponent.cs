using OpenTK;

namespace Subterran.OpenTK.Components
{
	public class MeshRendererComponent : EntityComponent, IRenderable
	{
		public ColoredVertex[] Vertices { get; set; }

		public void Render(Renderer renderer, Matrix4 matrix)
		{
			renderer.RenderMeshStreaming(ref matrix, Vertices);
		}
	}
}
