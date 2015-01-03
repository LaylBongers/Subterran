using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using Subterran.OpenTK.Components;

namespace Subterran.OpenTK
{
	public sealed class Renderer
	{
		private readonly int _matrixUniform;
		private readonly int _shaderProgram;
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

			// TODO: Clean things up below this line and split it into other files

			// Create a shader we'll render meshes with
			_shaderProgram = GL.CreateProgram();

			AttachShader(_shaderProgram, File.ReadAllText("./color.vert.glsl"), ShaderType.VertexShader);
			AttachShader(_shaderProgram, File.ReadAllText("./color.frag.glsl"), ShaderType.FragmentShader);

			GL.LinkProgram(_shaderProgram);

			// Report any errors found
			int linkStatus;
			GL.GetProgram(_shaderProgram, GetProgramParameterName.LinkStatus, out linkStatus);
			var log = GL.GetProgramInfoLog(_shaderProgram);
			if (linkStatus != 1)
			{
				var message = string.Format(null, "Shader program {0} failed to link!", _shaderProgram);
				Trace.TraceError(message);
				throw new ProgramException(message, log);
			}

			// If there's anything in the log, it might be a warning
			if (!string.IsNullOrEmpty(log))
			{
				Trace.TraceWarning("Shader program {0} compiled with warnings:", _shaderProgram);
				Trace.Indent();
				foreach (var logLine in log.Split('\n'))
				{
					Trace.TraceWarning(logLine);
				}
				Trace.Unindent();
			}

			// Set up shader uniforms
			_matrixUniform = GL.GetUniformLocation(_shaderProgram, "Matrix");

			if (_matrixUniform == -1)
			{
				var message = string.Format(null, "Shader program {0} does not contain required uniforms!", _shaderProgram);
				Trace.TraceError(message);
				throw new ProgramException(message);
			}

			Trace.TraceInformation("Created new shader program {0}!", _shaderProgram);
		}

		private static void AttachShader(int program, string source, ShaderType type)
		{
			var shader = GL.CreateShader(type);

			GL.ShaderSource(shader, source);
			GL.CompileShader(shader);

			int compileStatus;
			GL.GetShader(shader, ShaderParameter.CompileStatus, out compileStatus);
			if (compileStatus != 1)
			{
				var message = string.Format(null, "Shader {0} failed to compile!", shader);
				Trace.TraceWarning(message);
				throw new ShaderException(
					message,
					GL.GetShaderInfoLog(shader),
					source,
					type);
			}

			GL.AttachShader(program, shader);
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

		private static void CollapseEntityTreeTo(Entity entity, RenderData data, Matrix4 previousMatrix)
		{
			// Create a multiply matrix representing this entity
			var entityMatrix =
				Matrix4.CreateScale(entity.Scale)*
				Matrix4.CreateRotationX(entity.Rotation.X)*
				Matrix4.CreateRotationY(entity.Rotation.Y)*
				Matrix4.CreateRotationZ(entity.Rotation.Z)*
				Matrix4.CreateTranslation(entity.Position);

			// Multiply the model matrix with the previously created entity matrix
			previousMatrix = entityMatrix*previousMatrix;

			// Add all the entities we're interested in to the list
			data.Renderables.AddRange(
				entity
					.GetComponents<RenderEntityComponent>()
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

		public void RenderMeshStreaming(ref Matrix4 matrix, ColoredVertex[] vertices)
		{
			// Create a buffer for out vertex data, stream draw since we're streaming it
			var buffer = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ArrayBuffer, buffer);
			GL.BufferData(
				BufferTarget.ArrayBuffer,
				new IntPtr(ColoredVertex.SizeInBytes*vertices.Length),
				vertices,
				BufferUsageHint.StreamDraw);

			// Bind our shader program and give it the matrix uniform
			GL.UseProgram(_shaderProgram);
			GL.UniformMatrix4(_matrixUniform, false, ref matrix);

			// Tell OpenGL where to find the vertex attributes in the vertices
			GL.EnableVertexAttribArray(0);
			GL.VertexAttribPointer( // Vertices
				0, // attribute layout #0
				3, // size in type
				VertexAttribPointerType.Float, // type
				false, // normalize this attribute?
				ColoredVertex.SizeInBytes, // offset between start of vertex values (0 = tightly packed)
				0); // start offset

			GL.EnableVertexAttribArray(1);
			GL.VertexAttribPointer( // Colors
				1, // attribute layout #1
				3, // size in type
				VertexAttribPointerType.Float, // type
				false, // normalize this attribute?
				ColoredVertex.SizeInBytes, // offset between start of vertex values (0 = tightly packed)
				Vector3.SizeInBytes); // start offset

			// Actually draw!
			GL.DrawArrays(PrimitiveType.Triangles, 0, vertices.Length);

			// Clean everything up
			GL.DisableVertexAttribArray(0);
			GL.DisableVertexAttribArray(1);
			GL.DeleteBuffer(buffer);
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