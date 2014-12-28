namespace Subterran.OpenTK.Components
{
	public sealed class CameraComponent : EntityComponent
	{
		public ScreenPosition Position { get; set; }
		public ScreenSize Size { get; set; }

		public float VerticalFoV { get; set; }

		public float ZNear { get; set; }
		public float ZFar { get; set; }
	}
}