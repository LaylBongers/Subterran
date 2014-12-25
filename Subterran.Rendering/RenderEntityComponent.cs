using OpenTK;

namespace Subterran.Rendering
{
	public abstract class RenderEntityComponent : EntityComponent
	{
		public abstract void Render(Renderer renderer, Matrix4 matrix);
	}
}