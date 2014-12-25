using OpenTK;

namespace Subterran.Rendering.Components
{
	public class TestRenderComponent : RenderEntityComponent
	{
		public override void Render(Renderer renderer, Matrix4 matrix)
		{
			renderer.RenderMesh(ref matrix);
		}
	}
}