using OpenTK;

namespace Subterran.Rendering
{
	public interface IRenderable
	{
		void Render(Renderer renderer, Matrix4 matrix);
	}
}