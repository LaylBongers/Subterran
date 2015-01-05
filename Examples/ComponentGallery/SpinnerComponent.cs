using System;
using OpenTK;
using Subterran;

namespace ComponentGallery
{
	internal class SpinnerComponent : EntityComponent
	{
		public float Speed { get; set; }

		public override void Update(TimeSpan elapsed)
		{
			Entity.Rotation += new Vector3(0, elapsed.PerSecond(Speed), 0);
		}
	}
}