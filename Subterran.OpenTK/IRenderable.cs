using OpenTK;

namespace Subterran.OpenTK
{
	public interface IRenderable
	{
		void Render(Renderer renderer, Matrix4 matrix);
	}
}