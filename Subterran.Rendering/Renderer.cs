using System.Drawing;
using OpenTK.Graphics.OpenGL;

namespace Subterran.Rendering
{
	public class Renderer
	{
		private readonly Window _targetWindow;

		public Renderer(Window targetWindow)
		{
			_targetWindow = targetWindow;
		}

		public void RenderTest()
		{
			_targetWindow.MakeCurrent();

			// TODO: Make this OpenGL4
			GL.Begin(PrimitiveType.Triangles);

			GL.Vertex3(0, 1, 0);
			GL.Vertex3(-1, -1, 0);
			GL.Vertex3(1, -1, 0);

			GL.End();
		}

		public void Clear(Color color)
		{
			GL.ClearColor(color);
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
		}
	}
}