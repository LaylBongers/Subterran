using System;
using System.Drawing;
using System.Drawing.Imaging;
using OpenTK.Graphics.OpenGL4;
using GLPixelFormat = OpenTK.Graphics.OpenGL4.PixelFormat;
using PixelFormat = System.Drawing.Imaging.PixelFormat;

namespace Subterran.Rendering
{
	public sealed class Texture : IShaderUniformSettable
	{
		private readonly int _texture;

		public Texture(Bitmap bitmap)
		{
			if(bitmap == null)
				throw new ArgumentNullException("bitmap");

			// Save some meta-data
			Width = bitmap.Width;
			Height = bitmap.Height;

			// Load the data from the bitmap
			var textureData = bitmap.LockBits(
				new Rectangle(0, 0, bitmap.Width, bitmap.Height),
				ImageLockMode.ReadOnly,
				PixelFormat.Format32bppArgb);

			// Generate and bind a new OpenGL texture
			_texture = GL.GenTexture();
			GL.BindTexture(TextureTarget.Texture2D, _texture);

			// Configure the texture
			GL.TexParameter(TextureTarget.Texture2D,
				TextureParameterName.TextureMinFilter, (int) (TextureMinFilter.NearestMipmapLinear));
			GL.TexParameter(TextureTarget.Texture2D,
				TextureParameterName.TextureMagFilter, (int) (TextureMagFilter.Nearest));
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMaxLevel, 4);

			// Load the texture
			GL.TexImage2D(
				TextureTarget.Texture2D,
				0, // level
				PixelInternalFormat.Rgba,
				bitmap.Width, bitmap.Height,
				0, // border
				GLPixelFormat.Bgra,
				PixelType.UnsignedByte,
				textureData.Scan0);

			GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

			// Free up stuff we don't need anymore
			bitmap.UnlockBits(textureData);
			GL.BindTexture(TextureTarget.Texture2D, 0);
		}

		public int Width { get; set; }
		public int Height { get; set; }

		public void SetIn(Shader shader, string uniformName)
		{
			if(shader == null)
				throw new ArgumentNullException("shader");
			if (shader == null)
				throw new ArgumentNullException("uniformName");

			GL.ActiveTexture(TextureUnit.Texture0);
			GL.BindTexture(TextureTarget.Texture2D, _texture);
			shader.SetUniform(uniformName, 0);
		}

		public void DisposeSet()
		{
			GL.ActiveTexture(TextureUnit.Texture0);
			GL.BindTexture(TextureTarget.Texture2D, 0);
		}

		public static Texture FromPath(string path)
		{
			using (var bitmap = new Bitmap(path))
			{
				return new Texture(bitmap);
			}
		}
	}
}