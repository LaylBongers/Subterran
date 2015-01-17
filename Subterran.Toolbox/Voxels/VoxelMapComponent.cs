using System;
using Subterran.Rendering;
using Subterran.Rendering.Components;

namespace Subterran.Toolbox.Voxels
{
	public class VoxelMapComponent : EntityComponent, IInitializable, IRenderablePreparer
	{
		private bool _meshIsOutdated;
		private Voxel[,,] _voxels;
		private MeshRendererComponent _meshRenderer;

		public Voxel[,,] Voxels
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
			_meshRenderer = Entity.GetComponent<MeshRendererComponent>();

			if(_meshRenderer == null)
				throw new InvalidOperationException("This component requires a MeshRendererComponent!");
		}

		public void PrepareRender()
		{
			// If the mesh is outdated, we need to (re)generate it
			if (_meshIsOutdated)
			{
				_meshRenderer.Vertices = VoxelMapMesher.GenerateMesh(Voxels).ToArray();
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