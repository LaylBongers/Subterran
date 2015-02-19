namespace Subterran.Rendering
{
	internal interface IShaderUniformSettable
	{
		void Set(Shader shader, string uniformName);
		void DisposeSet();
	}
}