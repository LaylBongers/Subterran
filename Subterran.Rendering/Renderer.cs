using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Subterran.Rendering
{
	public sealed class Renderer
	{
		private readonly Window _targetWindow;

		public Renderer(Window targetWindow)
		{
			_targetWindow = targetWindow;
			_targetWindow.MakeCurrent();

			// Set up back-culling
			GL.Enable(EnableCap.CullFace);
			GL.CullFace(CullFaceMode.Back);
			GL.FrontFace(FrontFaceDirection.Ccw);
		}

		public void Clear(Color color)
		{
			GL.ClearColor(color);
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
		}

		public void RenderWorld(Entity world)
		{
			_targetWindow.MakeCurrent();

			// Collapse the world into renderable data
			var data = CollapseEntityTree(world);

			foreach (var renderable in data.Renderables)
			{
				renderable.Renderable.Render(this, renderable.Matrix);
			}
		}

		private static RenderData CollapseEntityTree(Entity entity)
		{
			var data = new RenderData();
			CollapseEntityTreeTo(entity, data);
			return data;
		}

		private static void CollapseEntityTreeTo(Entity entity, RenderData data)
		{
			// TODO: Actually get the mesh from the current entity if it exists
			data.Renderables.AddRange(entity
				.GetComponents<IRenderable>()
				.Select(c => new RenderableData
				{
					Matrix = Matrix4.Identity,
					Renderable = c
				}));

			foreach (var child in entity.Children)
			{
				CollapseEntityTreeTo(child, data);
			}
		}

		public void RenderMesh(Matrix4 modelMatrix)
		{
			// TODO: Actually make this do stuff other than render a triangle at 0,0,0 for every mesh

			GL.Begin(PrimitiveType.Triangles);

			GL.Vertex3(0, 1, 0);
			GL.Vertex3(-1, -1, 0);
			GL.Vertex3(1, -1, 0);

			GL.End();
		}

		private class RenderData
		{
			public RenderData()
			{
				Renderables = new List<RenderableData>();
			}

			public List<RenderableData> Renderables { get; set; }
		}

		private class RenderableData
		{
			public IRenderable Renderable { get; set; }
			public Matrix4 Matrix { get; set; }
		}
	}
}