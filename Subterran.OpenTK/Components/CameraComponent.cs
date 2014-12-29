using System.Drawing;

namespace Subterran.OpenTK.Components
{
	public sealed class CameraComponent : EntityComponent
	{
		public Point Position { get; set; }
		public Size Size { get; set; }

		public float VerticalFoV { get; set; }

		public float ZNear { get; set; }
		public float ZFar { get; set; }
	}
}