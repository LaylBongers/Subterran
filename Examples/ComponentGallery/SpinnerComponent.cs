using System;
using OpenTK;
using Subterran;
using Subterran.Toolbox;

namespace ComponentGallery
{
	internal class SpinnerComponent : EntityComponent, IUpdatable
	{
		public float Speed { get; set; }

		public void Update(TimeSpan elapsed)
		{
			Entity.Transform.Rotation += new Vector3(0, elapsed.PerSecond(Speed), 0);
		}
	}
}