using OpenTK;

namespace Subterran.Rendering
{
	public abstract class RenderEntityBehavior : EntityBehavior
	{
		public abstract void Render(Renderer renderer, Matrix4 matrix);
	}
}