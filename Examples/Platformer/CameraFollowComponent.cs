using System;
using OpenTK;
using Subterran;
using Subterran.Toolbox;

namespace Platformer
{
	internal class CameraFollowComponent : EntityComponent, IUpdatable
	{
		public Entity Target { get; set; }
		public int Distance { get; set; }
		public int HeightOffset { get; set; }

		public void Update(TimeSpan elapsed)
		{
			Entity.Transform.Position = Target.Transform.Position + new Vector3(0, HeightOffset, Distance);
		}
	}
}