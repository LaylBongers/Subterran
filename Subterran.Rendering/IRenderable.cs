using OpenTK;

namespace Subterran.Rendering
{
	internal interface IRenderable
	{
		void Render(Renderer renderer, Matrix4 modelMatrix);
	}
}