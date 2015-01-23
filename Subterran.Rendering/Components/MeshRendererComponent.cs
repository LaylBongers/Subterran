﻿using System;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace Subterran.Rendering.Components
{
	public class MeshRendererComponent : EntityComponent, IRenderable
	{
		private int _buffer = -1;
		private bool _bufferOutdated;
		private bool _streaming;
		private ColoredVertex[] _vertices;

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

		public ColoredVertex[] Vertices
		{
			get { return _vertices; }
			set
			{
				_vertices = value;
				MarkOutdated();
			}
		}

		public void Render(Renderer renderer, Matrix4 matrix)
		{
			// If we don't have a mesh (yet?) we just do nothing
			if (Vertices == null)
				return;

			if (Streaming)
			{
				// We're rendering it streaming so just send it over
				renderer.RenderMeshStreaming(ref matrix, Vertices);
			}
			else
			{
				RenderNonStreaming(renderer, ref matrix);
			}
		}

		private void RenderNonStreaming(Renderer renderer, ref Matrix4 matrix)
		{
			// Check if we have a valid buffer
			if (_buffer == -1 || _bufferOutdated)
			{
				// We don't have one or it is outdated, so update it
				UpdateBuffer();
			}

			// Now that we are sure we have a buffer, render it
			renderer.RenderBuffer(ref matrix, _buffer, Vertices.Length);
		}

		private void UpdateBuffer()
		{
			// Clear the old buffer if we have to
			if (_buffer != -1)
				GL.DeleteBuffer(_buffer);

			// Create a new buffer to store our vertices
			_buffer = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ArrayBuffer, _buffer);
			GL.BufferData(
				BufferTarget.ArrayBuffer,
				new IntPtr(ColoredVertex.SizeInBytes*Vertices.Length),
				Vertices,
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