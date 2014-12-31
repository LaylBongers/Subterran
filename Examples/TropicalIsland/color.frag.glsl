#version 330

// Data coming from the vertex shader
flat in vec3 fragColor;

// Output color, automatically gets picked up by OpenGL
out vec4 dv_FragColor;

void main()
{
	dv_FragColor = vec4(fragColor.rgb, 1.0f);
}