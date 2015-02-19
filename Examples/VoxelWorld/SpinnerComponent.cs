using System;
using Subterran;
using Subterran.Toolbox;

namespace VoxelWorld
{
	internal class SpinnerComponent : EntityComponent, IUpdatable
	{
		public void Update(TimeSpan elapsed)
		{
			var rotation = Entity.Transform.Rotation;
			rotation.Y += (float) elapsed.TotalSeconds*0.25f;

			if (rotation.Y > StMath.Tau)
			{
				rotation.Y -= StMath.Tau;
			}

			Entity.Transform.Rotation = rotation;
		}
	}
}