namespace Subterran.Rendering
{
	/// <summary>
	///     Used as interface on an EntityComponent.
	///     Will be called before IRenderable and allows you to prepare IRenderable components.
	///		This should only be used to prepare OTHER components than the IRenderablePreparer.
	/// </summary>
	public interface IRenderablePreparer
	{
		void PrepareRender();
	}
}