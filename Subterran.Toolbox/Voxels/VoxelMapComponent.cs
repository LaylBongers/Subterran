using System;
using Subterran.Rendering;
using Subterran.Rendering.Components;

namespace Subterran.Toolbox.Voxels
{
	public class VoxelMapComponent<TVoxelType> : EntityComponent, IInitializable, IRenderablePreparer
		where TVoxelType : struct
	{
		private bool _meshIsOutdated;
		private MeshRendererComponent _meshRenderer;
		private TVoxelType[,,] _voxels;

		public Func<TVoxelType[,,], ColoredVertex[]> MeshGenerator { get; set; }

		public TVoxelType[,,] Voxels
		{
			get { return _voxels; }
			set
			{
				_voxels = value;
				_meshIsOutdated = true;
			}
		}

		public void Initialize()
		{
			_meshIsOutdated = true;
			_meshRenderer = Entity.RequireComponent<MeshRendererComponent>();
		}

		public void PrepareRender()
		{
			if (MeshGenerator == null)
				throw new InvalidOperationException("This component requires MeshGenerator to be set!");

			// If the mesh is outdated, we need to (re)generate it
			if (_meshIsOutdated)
			{
				_meshRenderer.Vertices = MeshGenerator(Voxels);
				_meshIsOutdated = false;
			}
		}

		/// <summary>
		///     Marks the current background mesh of this voxel map as outdated.
		///     Use this if you want to re-generate the mesh after changing the voxel data.
		/// </summary>
		public void MarkMeshOutdated()
		{
			_meshIsOutdated = true;
		}
	}
}