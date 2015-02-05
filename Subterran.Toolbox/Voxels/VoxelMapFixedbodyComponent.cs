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
			var absoluteOffset = _renderer.Offset*scale;
			var voxelSize = Vector3.One*scale;

			// Get the locations the target falls within in the voxel map
			var bbXstart = StMath.Range((int) Math.Floor((target.Start.X - position.X - absoluteOffset.X)/scale.X), 0, width);
			var bbXend = StMath.Range((int) Math.Ceiling((target.End.X - position.X - absoluteOffset.X)/scale.X), 0, width);
			var bbYstart = StMath.Range((int) Math.Floor((target.Start.Y - position.Y - absoluteOffset.Y)/scale.Y), 0, height);
			var bbYend = StMath.Range((int) Math.Ceiling((target.End.Y - position.Y - absoluteOffset.Y)/scale.Y), 0, height);
			var bbZstart = StMath.Range((int) Math.Floor((target.Start.Z - position.Z - absoluteOffset.Z)/scale.Z), 0, depth);
			var bbZend = StMath.Range((int) Math.Ceiling((target.End.Z - position.Z - absoluteOffset.Z)/scale.Z), 0, depth);

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
						boundingBox.Start = (new Vector3(x, y, z)*scale) + absoluteOffset + position;
						boundingBox.End = boundingBox.Start + voxelSize;

						yield return boundingBox;
					}
				}
			}
		}
	}
}