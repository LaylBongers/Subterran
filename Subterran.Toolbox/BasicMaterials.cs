using Subterran.Rendering;
using Subterran.Toolbox.Materials;

namespace Subterran.Toolbox
{
	public static class BasicMaterials
	{
		public static Material<ColoredVertex, ColoredMaterialData> CreateFullbrightColor(Shader shader)
		{
			return new Material<ColoredVertex, ColoredMaterialData>
			{
				Shader = shader,
				Data = new ColoredMaterialData()
			};
		}

		public static Material<TexturedVertex, TexturedMaterialData> CreateFullbrightTexture(Shader shader, string path)
		{
			return new Material<TexturedVertex, TexturedMaterialData>
			{
				Shader = shader,
				Data = new TexturedMaterialData {Texture = Texture.FromPath(path)}
			};
		}
	}
}