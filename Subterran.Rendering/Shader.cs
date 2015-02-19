using System.Diagnostics;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace Subterran.Rendering
{
	public class Shader
	{
		private readonly int _program;

		public Shader(string vertexSource, string fragmentSource)
		{
			// Create a shader we'll render meshes with
			_program = GL.CreateProgram();

			ShaderUtils.AttachShader(_program, vertexSource, ShaderType.VertexShader);
			ShaderUtils.AttachShader(_program, fragmentSource, ShaderType.FragmentShader);
			GL.LinkProgram(_program);

			ShaderUtils.DetectErrors(_program);

			Trace.TraceInformation("Created new shader program {0}!", _program);
		}

		public void Use()
		{
			GL.UseProgram(_program);
		}

		public void SetUniform(string name, ref Matrix4 value)
		{
			// TODO: Allow some way of caching the lookup by name
			var location = ShaderUtils.GetUniformLocation(_program, name);
			GL.UniformMatrix4(location, false, ref value);
		}

		public void SetUniform(string name, int value)
		{
			var location = ShaderUtils.GetUniformLocation(_program, name);
			GL.Uniform1(location, value);
		}
	}
}