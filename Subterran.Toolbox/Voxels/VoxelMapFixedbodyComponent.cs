using System;
using System.Collections.Generic;
using OpenTK;
using Subterran.Toolbox.SimplePhysics;

namespace Subterran.Toolbox.Voxels
{
	public class VoxelMapFixedbodyComponent<TVoxelType> : EntityComponent, IInitializable, IFixedbodySource
		where TVoxelType : struct
	{
		private VoxelMapComponent<TVoxelType> _voxelMap;
		public Func<TVoxelType, bool> IsSolidChecker { get; set; }
		public IEnumerable<CubeCollider> Colliders { get; private set; }

		public void Initialize()
		{
			_voxelMap = Entity.RequireComponent<VoxelMapComponent<TVoxelType>>();
			Colliders = GenerateColliders();
		}

		private List<CubeCollider> GenerateColliders()
		{
			var colliders = new List<CubeCollider>();
			var voxels = _voxelMap.Voxels;
			var width = voxels.GetLength(0);
			var height = voxels.GetLength(1);
			var depth = voxels.GetLength(2);

			for (var x = 0; x < width; x++)
			{
				for (var z = 0; z < depth; z++)
				{
					var inCollider = false;
					var colliderStart = 0;

					for (var y = 0; y < height; y++)
					{
						// If the current voxel needs to be colliding
						if (IsSolidChecker(voxels[x, y, z]))
						{
							// If we're not yet creating a collider
							if (!inCollider)
							{
								inCollider = true;
								colliderStart = y;
							}

							// If we are creating a collider we don't need to do anything yet
						}
						else
						{
							// If we were in a collider
							if (inCollider)
							{
								colliders.Add(CreateColliderFor(colliderStart, y, x, z, Entity));
							}

							// If we weren't in a collider we don't need to do anything
						}
					}

					// We've gone through an entire pillar, a collider might have gone entirely to the top
					if (inCollider)
					{
						colliders.Add(CreateColliderFor(colliderStart, height, x, z, Entity));
					}
				}
			}

			return colliders;
		}

		private CubeCollider CreateColliderFor(int start, int end, int x, int z, Entity parent)
		{
			return new CubeCollider
			{
				Origin = new Vector3(x, start, z) * parent.Scale,
				Size = new Vector3(1, end - start, 1) * parent.Scale
			};
		}
	}
}