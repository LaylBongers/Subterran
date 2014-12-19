using OpenTK.Graphics.OpenGL;

namespace Subterran.Rendering
{
	public class Renderer
	{
		private readonly Window _window;

		private Renderer(Window window)
		{
			_window = window;
		}

		public void RenderTest()
		{
			// TODO: Make this OpenGL4
			GL.Begin(PrimitiveType.Triangles);

			GL.Vertex3(0, 1, 0);
			GL.Vertex3(-1, -1, 0);
			GL.Vertex3(1, -1, 0);

			GL.End();
		}

		public static Renderer For(Window window)
		{
			return new Renderer(window);
		}
	}
}