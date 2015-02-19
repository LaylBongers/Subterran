using Subterran.Rendering;

namespace Subterran.Toolbox
{
	public static class BasicShaders
	{
		private const string ColoredVertexShader = @"#version 330

// Transformation Matrices
uniform mat4 Matrix;

// Data coming into the vertex shader
layout(location = 0) in vec3 vertexPosition;
layout(location = 1) in vec3 vertexColor;

// Data going to the fragment shader
flat out vec3 fragColor;

void main()
{
	// Pass the color over to the fragment shader
	fragColor = vertexColor;

	gl_Position = Matrix * vec4(vertexPosition, 1.0);
}";
		private const string ColoredFragmentShader = @"#version 330

// Data coming from the vertex shader
flat in vec3 fragColor;

// Output color, automatically gets picked up by OpenGL
out vec4 st_FragColor;

void main()
{
	st_FragColor = vec4(fragColor.rgb, 1.0f);
}";
		private const string TexturedVertexShader = @"#version 330

// Transformation Matrices
uniform mat4 Matrix;

// Data coming into the vertex shader
layout(location = 0) in vec3 vertexPosition;
layout(location = 1) in vec2 vertexUv;

// Data going to the fragment shader
smooth out vec2 fragUv;

void main()
{
	// Pass the color over to the fragment shader
	fragUv = vertexUv;

	gl_Position = Matrix * vec4(vertexPosition, 1.0);
}";
		private const string TexturedFragmentShader = @"#version 330

// Textures
uniform sampler2D Texture;

// Data coming from the vertex shader
smooth in vec2 fragUv;

// Output color, automatically gets picked up by OpenGL
out vec4 st_FragColor;

void main()
{
	st_FragColor = texture(Texture, fragUv);
}";

		public static Shader CreateFullbrightColor()
		{
			return new Shader(ColoredVertexShader, ColoredFragmentShader);
		}

		public static Shader CreateFullbrightTexture()
		{
			return new Shader(TexturedVertexShader, TexturedFragmentShader);
		}
	}
}