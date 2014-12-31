using System;
using System.Runtime.Serialization;
using OpenTK.Graphics.OpenGL4;

namespace Subterran.OpenTK
{
	[Serializable]
	public class ProgramException : Exception, ISerializable
	{
		public ProgramException()
		{
		}

		public ProgramException(string message)
			: base(message)
		{
		}

		public ProgramException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		public ProgramException(string message, string programLog)
			: base(message)
		{
			ProgramLog = programLog;
		}

		protected ProgramException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			ProgramLog = info.GetString("ProgramLog");
		}

		public string ProgramLog { get; private set; }

		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue("ProgramLog", ProgramLog);
		}
	}

	[Serializable]
	public class ShaderException : Exception, ISerializable
	{
		public ShaderException()
		{
		}

		public ShaderException(string message)
			: base(message)
		{
		}

		public ShaderException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		public ShaderException(string message, string shaderLog, string shaderSource, ShaderType shaderType)
			: base(message)
		{
			ShaderLog = shaderLog;
			ShaderSource = shaderSource;
			ShaderType = shaderType;
		}

		protected ShaderException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			ShaderLog = info.GetString("ShaderLog");
			ShaderSource = info.GetString("ShaderSource");
			ShaderType = (ShaderType) info.GetInt32("ShaderType");
		}

		public string ShaderLog { get; private set; }
		public string ShaderSource { get; private set; }
		public ShaderType ShaderType { get; private set; }

		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue("ShaderLog", ShaderLog);
			info.AddValue("ShaderSource", ShaderSource);
			info.AddValue("ShaderType", (int) ShaderType);
		}
	}
}