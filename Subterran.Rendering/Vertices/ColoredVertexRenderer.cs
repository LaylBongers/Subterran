using System;
using System.Diagnostics;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace Subterran.Rendering.Vertices
{
	public sealed class ColoredVertexRenderer : IVertexRenderer<ColoredVertex>
	{
		private const string VertexShader = @"#version 330

// Transformation Matrices
uniform mat4 Matrix;

// Data coming into the vertex shader
layout(location = 0) in vec3 vertexPosition;
layout(location = 1) in vec3 vertexColor;

// Data going to the fragment shader
flat out vec3 fragColor;

void main()
{
	// Pass the color over to the fragment shader
	fragColor = vertexColor;

	gl_Position = Matrix * vec4(vertexPosition, 1.0);
}";
		private const string FragmentShader = @"#version 330

// Data coming from the vertex shader
flat in vec3 fragColor;

// Output color, automatically gets picked up by OpenGL
out vec4 st_FragColor;

void main()
{
	st_FragColor = vec4(fragColor.rgb, 1.0f);
}";
		private readonly int _matrixUniform;
		private readonly int _program;

		public ColoredVertexRenderer()
		{
			// Create a shader we'll render meshes with
			_program = GL.CreateProgram();

			ShaderUtils.AttachShader(_program, VertexShader, ShaderType.VertexShader);
			ShaderUtils.AttachShader(_program, FragmentShader, ShaderType.FragmentShader);
			GL.LinkProgram(_program);

			ShaderUtils.DetectErrors(_program);

			// Set up shader uniforms
			_matrixUniform = ShaderUtils.GetUniformLocation(_program, "Matrix");

			Trace.TraceInformation("Created new shader program {0}!", _program);
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

			RenderMeshBuffer(ref matrix, buffer, vertices.Length);

			GL.DeleteBuffer(buffer);
		}

		public void RenderMeshBuffer(ref Matrix4 matrix, int buffer, int length)
		{
			GL.BindBuffer(BufferTarget.ArrayBuffer, buffer);

			// Bind our shader program and give it the matrix uniform
			GL.UseProgram(_program);
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
			GL.DrawArrays(PrimitiveType.Triangles, 0, length);

			// Clean everything up
			GL.DisableVertexAttribArray(0);
			GL.DisableVertexAttribArray(1);
		}
	}
}