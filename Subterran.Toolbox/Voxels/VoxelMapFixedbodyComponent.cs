using System;
using System.Collections.Generic;
using OpenTK;
using Subterran.Toolbox.SimplePhysics;

namespace Subterran.Toolbox.Voxels
{
	public class VoxelMapFixedbodyComponent<TVoxelType> : EntityComponent, ISmartFixedbodySource
		where TVoxelType : struct
	{
		public Func<TVoxelType, bool> IsSolidChecker { get; set; }
		public Vector3 Offset { get; set; }
		public TVoxelType[,,] Voxels { get; set; }

		public IEnumerable<BoundingBox> GetBoundingBoxesWithin(BoundingBox collisionArea)
		{
			if (Entity.Transform.Rotation != Vector3.Zero)
				throw new InvalidOperationException("VoxelMapFixedbodyComponent does not support rotation!");
			if (Voxels == null)
				throw new InvalidOperationException("VoxelMapFixedbodyComponent requires Voxels to be set!");

			var voxels = Voxels;
			var width = voxels.GetLength(0);
			var height = voxels.GetLength(1);
			var depth = voxels.GetLength(2);
			var size = new Vector3i(width, height, depth);

			var position = Entity.Transform.Position;
			var scale = Entity.Transform.Scale;
			var inverseScale = Entity.Transform.InverseScale;
			var voxelSize = Vector3.One*scale;
			// Full offset from origin to voxel axes origin
			var axisOffset = position + (Offset*scale);

			// Sometimes a Math.Floor results in 4.999 instead of 5, compensate for this.
			// This will in many situations cause too many blocks to be selected,
			// but the alternative of blocks not being selected is worse.
			var floatErrorCompensation = new Vector3(0.001f);

			// Get the locations the target falls within in the voxel map
			var bbStart = StMath.Range(
				StMath.Floor((collisionArea.Start - axisOffset)*inverseScale - floatErrorCompensation),
				new Vector3i(0), size);
			var bbEnd = StMath.Range(
				StMath.Ceiling((collisionArea.End - axisOffset)*inverseScale + floatErrorCompensation),
				new Vector3i(0), size);

			// Go through all the voxels that fall within that area
			for (var x = bbStart.X; x < bbEnd.X; x++)
			{
				for (var y = bbStart.Y; y < bbEnd.Y; y++)
				{
					for (var z = bbStart.Z; z < bbEnd.Z; z++)
					{
						if (!IsSolidChecker(voxels[x, y, z]))
							continue;

						var boundingBox = new BoundingBox();
						boundingBox.Start = (new Vector3(x, y, z)*scale) + axisOffset;
						boundingBox.End = boundingBox.Start + voxelSize;

						yield return boundingBox;
					}
				}
			}
		}
	}
}