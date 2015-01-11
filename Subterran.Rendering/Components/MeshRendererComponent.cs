using System;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace Subterran.Rendering.Components
{
	public class MeshRendererComponent : EntityComponent, IRenderable
	{
		private int _buffer = -1;

		public ColoredVertex[] Vertices { get; set; }
		public bool Streaming { get; set; }

		// TODO: Move streaming meshes to a separate component for clarity.
		public bool BufferOutdated { get; set; }

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
				// We're not streaming, so check if we have a valid buffer
				if (_buffer == -1 || BufferOutdated)
				{
					// We don't, so create one
					_buffer = GL.GenBuffer();
					GL.BindBuffer(BufferTarget.ArrayBuffer, _buffer);
					GL.BufferData(
						BufferTarget.ArrayBuffer,
						new IntPtr(ColoredVertex.SizeInBytes*Vertices.Length),
						Vertices,
						BufferUsageHint.StaticDraw);

					BufferOutdated = false;
				}

				// Now that we have a buffer, render it
				renderer.RenderBuffer(ref matrix, _buffer, Vertices.Length);
			}
		}
	}
}
