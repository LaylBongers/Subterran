using System.Globalization;
using System.IO;
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
				var message = string.Format(CultureInfo.InvariantCulture,
					ExceptionMessages.ShaderUtils_ShaderFailedCompile, shader);

				StLogging.Error(message);
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
				var message = string.Format(CultureInfo.InvariantCulture,
					ExceptionMessages.ShaderUtils_ProgramFailedLinking, program);

				StLogging.Error(message);
				throw new ProgramException(message, log);
			}

			// If there's anything in the log, it might be a warning
			if (!string.IsNullOrEmpty(log))
			{
				using (var message = new StringWriter(CultureInfo.InvariantCulture))
				{
					message.WriteLine(ExceptionMessages.ShaderUtils_ProgramDoesNotContainUniform, program);
					foreach (var logLine in log.Split('\n'))
					{
						message.WriteLine("  " + logLine);
					}

					StLogging.Warning(message.ToString());
				}
			}
		}

		public static int GetUniformLocation(int program, string name)
		{
			var value = GL.GetUniformLocation(program, name);

			if (value != -1)
				return value;

			var message = string.Format(CultureInfo.InvariantCulture,
				ExceptionMessages.ShaderUtils_ProgramDoesNotContainUniform, program, name);

			StLogging.Error(message);
			throw new ProgramException(message);
		}
	}
}