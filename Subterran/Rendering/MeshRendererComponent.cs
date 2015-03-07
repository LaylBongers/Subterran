using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace Subterran.Rendering
{
	public class MeshRendererComponent<TVertexType> : EntityComponent, IRenderable
		where TVertexType : struct 
	{
		private int _buffer = -1;
		private bool _bufferOutdated;
		private bool _streaming;
		private TVertexType[] _vertices;

		public Vector3 Offset { get; set; }

		/// <summary>
		///     If true, renders the mesh by streaming the vertices every
		///     frame rather than maintaining a cached buffer.
		/// </summary>
		public bool Streaming
		{
			get { return _streaming; }
			set
			{
				_streaming = value;
				UpdateStreaming();
			}
		}
		
		public void SetMesh(TVertexType[] vertices)
		{
			_vertices = vertices;
			MarkOutdated();
		}

		public Material<TVertexType> Material { get; set; }

		public event EventHandler StartedRender = (s, e) => { };

		public void Render(Renderer renderer, Matrix4 matrix)
		{
			 StartedRender(this, EventArgs.Empty);

			// If we don't have a mesh (yet?) we just do nothing
			if (_vertices == null)
				return;

			// If we're streaming or not, if we have an offset we need to add that to the matrix
			if (Offset != Vector3.Zero)
			{
				matrix = Matrix4.CreateTranslation(Offset) * matrix;
			}
			
			if (Streaming)
			{
				// We're rendering it streaming so just send it over
				throw new NotImplementedException();
			}
			else
			{
				RenderNonStreaming(ref matrix);
			}
		}

		[SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference")]
		private void RenderNonStreaming(ref Matrix4 matrix)
		{
			// Check if we have a valid buffer
			if (_buffer == -1 || _bufferOutdated)
			{
				// We don't have one or it is outdated, so update it
				UpdateBuffer();
			}

			// Now that we are sure we have a buffer, render it
			Material.RenderBuffer(ref matrix, _buffer, _vertices.Length);
		}

		private void UpdateBuffer()
		{
			// Clear the old buffer if we have to
			if (_buffer != -1)
				GL.DeleteBuffer(_buffer);

			var vertexSize = Marshal.SizeOf(typeof (TVertexType));

			// Create a new buffer to store our vertices
			_buffer = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ArrayBuffer, _buffer);
			GL.BufferData(
				BufferTarget.ArrayBuffer,
				new IntPtr(vertexSize * _vertices.Length),
				_vertices,
				BufferUsageHint.StaticDraw);

			_bufferOutdated = false;
		}

		private void UpdateStreaming()
		{
			if (Streaming && _buffer != -1)
			{
				// We're streaming but we have a buffer, clean it up
				GL.DeleteBuffer(_buffer);
				_buffer = -1;
			}
		}

		public void MarkOutdated()
		{
			_bufferOutdated = true;
		}
	}
}