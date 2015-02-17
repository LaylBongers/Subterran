using System;
using System.Diagnostics;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace Subterran.Rendering.Vertices
{
	public sealed class TexturedVertexRenderer : IVertexRenderer<TexturedVertex>
	{
		private const string VertexShader = @"#version 330

// Transformation Matrices
uniform mat4 Matrix;

// Data coming into the vertex shader
layout(location = 0) in vec3 vertexPosition;
layout(location = 1) in vec2 vertexUv;

// Data going to the fragment shader
smooth out vec2 fragUv;

void main()
{
	// Pass the color over to the fragment shader
	fragUv = vertexUv;

	gl_Position = Matrix * vec4(vertexPosition, 1.0);
}";
		private const string FragmentShader = @"#version 330

// Textures
uniform sampler2D TextureSampler;

// Data coming from the vertex shader
smooth in vec2 fragUv;

// Output color, automatically gets picked up by OpenGL
out vec4 st_FragColor;

void main()
{
	st_FragColor = texture(TextureSampler, fragUv);
}";
		private readonly int _matrixUniform;
		private readonly int _program;

		public TexturedVertexRenderer()
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

		public void RenderMeshStreaming(ref Matrix4 matrix, TexturedVertex[] vertices)
		{
			throw new NotImplementedException();
		}

		public void RenderMeshBuffer(ref Matrix4 matrix, int buffer, int length)
		{
			throw new NotImplementedException();
		}
	}
}