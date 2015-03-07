namespace Subterran.Rendering
{
	public interface IShaderUniformSettable
	{
		void SetIn(Shader shader, string uniformName);
		void DisposeSet();
	}
}