using System;
using Subterran.Rendering;
using Subterran.WorldState;

namespace Subterran.Toolbox.Voxels
{
	public class VoxelMapRendererComponent<TVoxelType, TVertexType> : EntityComponent, IInitializable
		where TVoxelType : struct
		where TVertexType : struct
	{
		private bool _meshIsOutdated;
		private MeshRendererComponent<TVertexType> _meshRenderer;
		private TVoxelType[,,] _voxels;
		public Func<TVoxelType[,,], TVertexType[]> MeshGenerator { get; set; }

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
			_meshRenderer = Entity.RequireOne<MeshRendererComponent<TVertexType>>();
			_meshRenderer.StartedRender += StartedRender;
		}

		public event EventHandler StartRender = (s, e) => { };

		private void StartedRender(object sender, EventArgs e)
		{
			StartRender(this, EventArgs.Empty);

			if (Voxels == null)
				throw new InvalidOperationException("VoxelMapRendererComponent requires Voxels to be set!");
			if (MeshGenerator == null)
				throw new InvalidOperationException("VoxelMapRendererComponent requires MeshGenerator to be set!");

			// If the mesh is outdated, we need to (re)generate it
			if (_meshIsOutdated)
			{
				_meshRenderer.SetMesh(MeshGenerator(Voxels));
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