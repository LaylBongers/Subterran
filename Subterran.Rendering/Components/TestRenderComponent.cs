using OpenTK;

namespace Subterran.Rendering.Components
{
	public class TestRenderComponent : EntityComponent, IRenderable
	{
		public void Render(Renderer renderer, Matrix4 modelMatrix)
		{
			renderer.RenderMesh(modelMatrix);
		}
	}
}