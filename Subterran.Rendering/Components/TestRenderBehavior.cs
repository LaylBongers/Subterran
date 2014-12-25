using OpenTK;

namespace Subterran.Rendering.Components
{
	public class TestRenderBehavior : RenderEntityBehavior
	{
		public override void Render(Renderer renderer, Matrix4 matrix)
		{
			renderer.RenderMesh(ref matrix);
		}
	}
}