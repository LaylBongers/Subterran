namespace Subterran.Rendering
{
	public interface IShaderUniformSettable
	{
		void Set(Shader shader, string uniformName);
		void DisposeSet();
	}
}