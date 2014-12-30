using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using Subterran.OpenTK.Components;

namespace Subterran.OpenTK
{
	public sealed class Renderer
	{
		private readonly Window _targetWindow;

		public Renderer(Window targetWindow)
		{
			_targetWindow = targetWindow;
			_targetWindow.MakeCurrent();

			GL.Enable(EnableCap.DepthTest);

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
				// If the size is a Zero, we need to default to full screen
				var size = camera.Component.Size == Size.Empty
					? _targetWindow.Size
					: camera.Component.Size;

				// Set the viewport where we will render to on the screen
				GL.Viewport(camera.Component.Position, size);

				// Set up the matrices we will need to actually render
				var projection = Matrix4.CreatePerspectiveFieldOfView(
					camera.Component.VerticalFoV, (float) size.Width/size.Height,
					camera.Component.ZNear, camera.Component.ZFar);
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
			var entityMatrix =
				Matrix4.CreateRotationX(entity.Rotation.X)*
				Matrix4.CreateRotationY(entity.Rotation.Y)*
				Matrix4.CreateRotationZ(entity.Rotation.Z)*
				Matrix4.CreateTranslation(entity.Position);

			// Multiply the model matrix with the previously created entity matrix
			modelMatrix = entityMatrix*modelMatrix;

			// Add all the entities we're interested in to the list
			data.Renderables.AddRange(
				entity
					.GetComponents<RenderEntityComponent>()
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

			GL.Vertex3(-0.5f, 0.5f, 0); // Left Top
			GL.Vertex3(-0.5f, -0.5f, 0); // Left Bottom
			GL.Vertex3(0.5f, -0.5f, 0); // Right Bottom

			GL.Vertex3(-0.5f, 0.5f, 0); // Left Top
			GL.Vertex3(0.5f, -0.5f, 0); // Right Bottom
			GL.Vertex3(0.5f, 0.5f, 0); // Right Top

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
			public RenderEntityComponent Component { get; set; }
			public Matrix4 Matrix { get; set; }
		}
	}
}