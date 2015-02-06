using System;
using System.Collections.Generic;
using OpenTK;
using Subterran.Rendering.Components;
using Subterran.Toolbox.SimplePhysics;

namespace Subterran.Toolbox.Voxels
{
	public class VoxelMapFixedbodyComponent<TVoxelType> : EntityComponent, IInitializable, ISmartFixedbodySource
		where TVoxelType : struct
	{
		private MeshRendererComponent _renderer;
		private VoxelMapComponent<TVoxelType> _voxelMap;
		public Func<TVoxelType, bool> IsSolidChecker { get; set; }

		public void Initialize()
		{
			_voxelMap = Entity.RequireComponent<VoxelMapComponent<TVoxelType>>();
			_renderer = Entity.RequireComponent<MeshRendererComponent>();
		}

		public IEnumerable<BoundingBox> GetPotentialBoundingBoxes(BoundingBox target)
		{
			if (Entity.Rotation != Vector3.Zero)
				throw new InvalidOperationException("VoxelMapFixedbodyComponent does not support rotation!");

			var voxels = _voxelMap.Voxels;
			var width = voxels.GetLength(0);
			var height = voxels.GetLength(1);
			var depth = voxels.GetLength(2);

			var position = Entity.Position;
			var scale = Entity.Scale;
			var inverseScale = Entity.InverseScale;
			var voxelSize = Vector3.One*scale;
			// Full offset from origin to voxel axes origin
			var axisOffset = position + (_renderer.Offset*scale);

			// Get the locations the target falls within in the voxel map
			var bbStart = (target.Start - axisOffset)*inverseScale;
			var bbEnd = (target.End - axisOffset)*inverseScale;

			// Sometimes a Math.Floor results in 4.999 instead of 5, compensate for this.
			// This will in many situations cause too many blocks to be selected,
			// but the alternative of blocks not being selected is worse.
			const float floatErrorCompensation = 0.1f;
			var bbXstart = StMath.Range((int) (Math.Floor(bbStart.X) - floatErrorCompensation), 0, width);
			var bbXend = StMath.Range((int) (Math.Ceiling(bbEnd.X) + floatErrorCompensation), 0, width);
			var bbYstart = StMath.Range((int) (Math.Floor(bbStart.Y) - floatErrorCompensation), 0, height);
			var bbYend = StMath.Range((int) (Math.Ceiling(bbEnd.Y) + floatErrorCompensation), 0, height);
			var bbZstart = StMath.Range((int) (Math.Floor(bbStart.Z) - floatErrorCompensation), 0, depth);
			var bbZend = StMath.Range((int) (Math.Ceiling(bbEnd.Z) + floatErrorCompensation), 0, depth);

			// Go through all the voxels that fall within that area
			for (var x = bbXstart; x < bbXend; x++)
			{
				for (var y = bbYstart; y < bbYend; y++)
				{
					for (var z = bbZstart; z < bbZend; z++)
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