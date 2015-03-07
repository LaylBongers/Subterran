using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
		private readonly Action _attributeDisabler;
		private readonly List<Action> _attributeEnablers = new List<Action>();
		private readonly List<Action<TDataType>> _uniformDisposers = new List<Action<TDataType>>();
		private readonly List<Action<TDataType, Shader>> _uniformSetters = new List<Action<TDataType, Shader>>();

		public Material()
		{
			var dataType = typeof (TDataType);

			// Create the list of uniform setter actions
			// These will set the uniforms in our shader with the properties in Data
			foreach (var property in dataType.GetProperties())
			{
				var name = property.Name;
				var getter = property.GetGetMethod();

				// If the getter's return type can be assigned to a IShaderUniformSettable
				if (typeof (IShaderUniformSettable).IsAssignableFrom(getter.ReturnType))
				{
					AddSetterAndDisposerForGetter(getter, name);
				}
			}

			// Create the list of attribute setter actions
			var count = 0;
			var vertexType = typeof (TVertexType);
			var sizeInBytes = Marshal.SizeOf(vertexType);

			// Get all fields that are public and not static
			foreach (var field in vertexType.GetFields().Where(f => f.IsPublic && !f.IsStatic))
			{
				var layoutNumber = count;
				var fieldSizeInDataTypes = Marshal.SizeOf(field.FieldType)/4;
				var fieldOffsetInBytes = Marshal.OffsetOf(vertexType, field.Name);

				_attributeEnablers.Add(() =>
				{
					// Set up the vertex attribute for this field
					GL.EnableVertexAttribArray(layoutNumber);
					GL.VertexAttribPointer(
						// Layout number
						layoutNumber,
						// Size of attribute in amount of the data type
						// For example, Vector3 = 3, because it's 3 floats big
						fieldSizeInDataTypes,
						// Data type
						VertexAttribPointerType.Float,
						// Should value be normalized
						false,
						// Offset between two starts of this attribute
						sizeInBytes,
						// Offset of first value of this attribute
						fieldOffsetInBytes);
				});

				count++;
			}

			_attributeDisabler = () =>
			{
				for (var i = 0; i < count; i++)
				{
					GL.DisableVertexAttribArray(0);
				}
			};
		}

		public Shader Shader { get; set; }
		public TDataType Data { get; set; }

		private void AddSetterAndDisposerForGetter(MethodInfo getter, string uniformName)
		{
			_uniformSetters.Add((d, s) =>
			{
				var value = (IShaderUniformSettable) getter.Invoke(d, new object[0]);
				value.Set(Shader, uniformName);
			});
			_uniformDisposers.Add(d =>
			{
				var value = (IShaderUniformSettable) getter.Invoke(d, new object[0]);
				value.DisposeSet();
			});
		}

		public override void RenderBuffer(ref Matrix4 matrix, int buffer, int length)
		{
			GL.BindBuffer(BufferTarget.ArrayBuffer, buffer);

			Shader.Use();
			Shader.SetUniform("Matrix", ref matrix);

			SetUniforms();
			EnableAttributes();

			GL.DrawArrays(PrimitiveType.Triangles, 0, length);

			DisableAttributes();
			UnsetUniforms();
		}

		private void SetUniforms()
		{
			foreach (var setter in _uniformSetters)
			{
				setter(Data, Shader);
			}
		}

		private void UnsetUniforms()
		{
			foreach (var disposer in _uniformDisposers)
			{
				disposer(Data);
			}
		}

		private void EnableAttributes()
		{
			foreach (var attributeEnabler in _attributeEnablers)
			{
				attributeEnabler();
			}
		}

		private void DisableAttributes()
		{
			_attributeDisabler();
		}
	}
}