using System.Diagnostics;
using OpenTK.Graphics.OpenGL4;

namespace Subterran.Rendering
{
	public static class ShaderUtils
	{
		public static void AttachShader(int program, string source, ShaderType type)
		{
			var shader = GL.CreateShader(type);

			GL.ShaderSource(shader, source);
			GL.CompileShader(shader);

			int compileStatus;
			GL.GetShader(shader, ShaderParameter.CompileStatus, out compileStatus);
			if (compileStatus != 1)
			{
				var message = string.Format("Shader {0} failed to compile!", shader);
				Trace.TraceWarning(message);
				throw new ShaderException(
					message,
					GL.GetShaderInfoLog(shader),
					source,
					type);
			}

			GL.AttachShader(program, shader);
		}

		public static void DetectErrors(int program)
		{
			// Report any errors found
			int linkStatus;
			GL.GetProgram(program, GetProgramParameterName.LinkStatus, out linkStatus);
			var log = GL.GetProgramInfoLog(program);
			if (linkStatus != 1)
			{
				var message = string.Format("Shader program {0} failed to link!", program);
				Trace.TraceError(message);
				throw new ProgramException(message, log);
			}

			// If there's anything in the log, it might be a warning
			if (!string.IsNullOrEmpty(log))
			{
				Trace.TraceWarning("Shader program {0} compiled with warnings:", program);
				Trace.Indent();
				foreach (var logLine in log.Split('\n'))
				{
					Trace.TraceWarning(logLine);
				}
				Trace.Unindent();
			}
		}

		public static int GetUniformLocation(int program, string name)
		{
			var value = GL.GetUniformLocation(program, name);

			if (value != -1)
				return value;

			var message = string.Format("Shader program {0} does not contain \"{1}\" uniform!", program, name);
			Trace.TraceError(message);
			throw new ProgramException(message);
		}
	}
}
