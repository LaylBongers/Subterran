using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using Subterran.Rendering.Components;

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

			// For each camera
			foreach (var camera in data.Cameras)
			{
				var projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(70), 1920f/1080f, 0.1f, 100);
				var projectionView = camera.Matrix.Inverted()*projection;

				// Render all renderable things
				foreach (var renderable in data.Renderables)
				{
					renderable.Component.Render(this, renderable.Matrix*projectionView);
				}
			}
		}

		private static RenderData CollapseEntityTree(Entity entity)
		{
			var data = new RenderData();
			CollapseEntityTreeTo(entity, data, Matrix4.Identity);
			return data;
		}

		private static void CollapseEntityTreeTo(Entity entity, RenderData data, Matrix4 modelMatrix)
		{
			// Create a multiply matrix representing this entity
			var rotation = entity.Transform.Rotation;
			var entityMatrix =
				Matrix4.CreateRotationX(rotation.X)*
				Matrix4.CreateRotationY(rotation.Y)*
				Matrix4.CreateRotationZ(rotation.Z)*
				Matrix4.CreateTranslation(entity.Transform.Position.ToVector3());

			// Multiply the model matrix with the previously created entity matrix
			modelMatrix = entityMatrix*modelMatrix;

			// Add all the entities we're interested in to the list
			data.Renderables.AddRange(
				entity
					.GetComponents<IRenderable>()
					.Select(c => new RenderableData
					{
						Matrix = modelMatrix,
						Component = c
					})
				);
			data.Cameras.AddRange(
				entity
					.GetComponents<CameraComponent>()
					.Select(c => new CameraData
					{
						Matrix = modelMatrix,
						Component = c
					})
				);

			// Recursively do the same for every child entity
			foreach (var child in entity.Children)
			{
				CollapseEntityTreeTo(child, data, modelMatrix);
			}
		}

		public void RenderMesh(ref Matrix4 modelMatrix)
		{
			// TODO: Actually make this do stuff other than render a triangle for every mesh

			GL.MatrixMode(MatrixMode.Modelview);
			GL.LoadMatrix(ref modelMatrix);

			GL.Begin(PrimitiveType.Triangles);

			GL.Vertex3(-1, 1, 0); // Left Top
			GL.Vertex3(-1, -1, 0); // Left Bottom
			GL.Vertex3(1, -1, 0); // Right Bottom

			GL.Vertex3(-1, 1, 0); // Left Top
			GL.Vertex3(1, -1, 0); // Right Bottom
			GL.Vertex3(1, 1, 0); // Right Top

			GL.End();
		}

		private class CameraData
		{
			public CameraComponent Component { get; set; }
			public Matrix4 Matrix { get; set; }
		}

		private class RenderData
		{
			public RenderData()
			{
				Renderables = new List<RenderableData>();
				Cameras = new List<CameraData>();
			}

			public List<RenderableData> Renderables { get; set; }
			public List<CameraData> Cameras { get; set; }
		}

		private class RenderableData
		{
			public IRenderable Component { get; set; }
			public Matrix4 Matrix { get; set; }
		}
	}
}