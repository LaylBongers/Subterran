using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace Subterran.Rendering
{
	public abstract class Material<TVertexType>
		where TVertexType : struct
	{
		internal Material()
		{
		}

		public abstract void RenderBuffer(ref Matrix4 matrix, int buffer, int length);
	}

	public sealed class Material<TVertexType, TDataType> : Material<TVertexType>
		where TVertexType : struct
		where TDataType : class
	{
		public Shader Shader { get; set; }
		public TDataType Data { get; set; }

		public override void RenderBuffer(ref Matrix4 matrix, int buffer, int length)
		{
			GL.BindBuffer(BufferTarget.ArrayBuffer, buffer);

			Shader.Use();
			Shader.SetUniform("Matrix", ref matrix);

			SetUniforms();
			var amount = EnableAttributes();

			GL.DrawArrays(PrimitiveType.Triangles, 0, length);

			DisableAttributes(amount);
			UnsetUniforms();
		}

		private void SetUniforms()
		{
			var type = typeof(TDataType);

			// Get all properties from the data object
			foreach (var property in type.GetProperties())
			{
				var get = property.GetGetMethod();
				var value = get.Invoke(Data, new object[0]) as IShaderUniformSettable;

				if (value != null)
				{
					value.Set(Shader, property.Name);
				}
			}
		}

		private void UnsetUniforms()
		{
			var type = typeof(TDataType);

			// Get all properties from the data object
			foreach (var property in type.GetProperties())
			{
				var get = property.GetGetMethod();
				var value = get.Invoke(Data, new object[0]) as IShaderUniformSettable;

				if (value != null)
				{
					value.DisposeSet();
				}
			}
		}

		private int EnableAttributes()
		{
			var count = 0;
			var type = typeof (TVertexType);
			var sizeInBytes = Marshal.SizeOf(type);
			
			// Get all fields that are public and not static
			foreach (var field in type.GetFields().Where(f => f.IsPublic && !f.IsStatic))
			{
				// Set up the vertex attribute for this field
				GL.EnableVertexAttribArray(count);
				GL.VertexAttribPointer(
					// Layout number
					count,
					// Size of attribute in amount of the data type
					// For example, Vector3 = 3, because it's 3 floats big
					Marshal.SizeOf(field.FieldType) / 4,
					// Data type
					VertexAttribPointerType.Float,
					// Should value be normalized
					false,
					// Offset between two starts of this attribute
					sizeInBytes,
					// Offset of first value of this attribute
					Marshal.OffsetOf(type, field.Name));

				count++;
			}

			return count;
		}

		private void DisableAttributes(int amount)
		{
			for (var i = 0; i < amount; i++)
			{
				GL.DisableVertexAttribArray(0);
			}
		}
	}
}