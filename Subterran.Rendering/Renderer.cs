using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using Subterran.Rendering.Components;
using Subterran.Rendering.Vertices;

namespace Subterran.Rendering
{
	public sealed class Renderer
	{
		private readonly Window _targetWindow;
		private readonly Dictionary<Type, IVertexRenderer> _vertexRenderers = new Dictionary<Type, IVertexRenderer>();

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

		public void RegisterVertexType<TVertex>(IVertexRenderer<TVertex> renderer)
		{
			_vertexRenderers.Add(typeof (TVertex), renderer);
		}

		private IVertexRenderer<TVertex> RetrieveVertexRenderer<TVertex>()
		{
			IVertexRenderer renderer;
			if (!_vertexRenderers.TryGetValue(typeof (TVertex), out renderer))
				throw new InvalidOperationException("No vertex renderer has been registered for this vertex type!");
			return (IVertexRenderer<TVertex>) renderer;
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
				var position = camera.Component.Position;
				var size = camera.Component.Size == Size.Empty
					? _targetWindow.Size
					: camera.Component.Size;

				// Set the viewport where we will render to on the screen
				GL.Viewport(position, size);

				// Clear the viewport so there's no old data left
				GL.Enable(EnableCap.ScissorTest);
				GL.Scissor(position.X, position.Y, size.Width, size.Height);
				GL.ClearColor(camera.Component.Color);
				GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
				GL.Disable(EnableCap.ScissorTest);

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

		public void RenderMeshStreaming<TVertex>(ref Matrix4 matrix, TVertex[] vertices)
		{
			var renderer = RetrieveVertexRenderer<TVertex>();
			renderer.RenderMeshStreaming(ref matrix, vertices);
		}

		public void RenderMeshBuffer<TVertex>(ref Matrix4 matrix, int buffer, int length)
		{
			var renderer = RetrieveVertexRenderer<TVertex>();
			renderer.RenderMeshBuffer(ref matrix, buffer, length);
		}

		private static RenderData CollapseEntityTree(Entity entity)
		{
			var data = new RenderData();
			CollapseEntityTreeTo(entity, data, Matrix4.Identity);
			return data;
		}

		private static void CollapseEntityTreeTo(Entity entity, RenderData data, Matrix4 previousMatrix)
		{
			// Create a multiply matrix representing this entity
			// TODO: Right now i'm refactoring, come back to this and fix this.
			var entityMatrix =
				Matrix4.CreateScale(entity.Transform.Scale)*
				Matrix4.CreateRotationX(entity.Transform.Rotation.X)*
				Matrix4.CreateRotationY(entity.Transform.Rotation.Y)*
				Matrix4.CreateRotationZ(entity.Transform.Rotation.Z)*
				Matrix4.CreateTranslation(entity.Transform.Position);

			// Multiply the model matrix with the previously created entity matrix
			previousMatrix = entityMatrix*previousMatrix;

			// Add all the entities we're interested in to the list
			data.Renderables.AddRange(
				entity
					.GetComponents<IRenderable>()
					.Select(c => new RenderableData
					{
						Matrix = previousMatrix,
						Component = c
					})
				);
			data.Cameras.AddRange(
				entity
					.GetComponents<CameraComponent>()
					.Select(c => new CameraData
					{
						Matrix = previousMatrix,
						Component = c
					})
				);

			// Recursively do the same for every child entity
			foreach (var child in entity.Children)
			{
				CollapseEntityTreeTo(child, data, previousMatrix);
			}
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