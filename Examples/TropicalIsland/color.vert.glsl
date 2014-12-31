#version 330

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
}