using OpenTK;
using Subterran.Rendering;

namespace Subterran.Toolbox.Voxels
{
	public class FixedSizeVoxelMapComponent : EntityComponent, IRenderable
	{
		private bool _meshIsOutdated;
		private ColoredVertex[] _vertices;
		private Voxel[,,] _voxels;

		public FixedSizeVoxelMapComponent()
			: this(0, 0, 0)
		{
		}

		public FixedSizeVoxelMapComponent(int width, int height, int depth)
		{
			Voxels = new Voxel[width, height, depth];
		}

		public Voxel[,,] Voxels
		{
			get { return _voxels; }
			set
			{
				// Bug: The mesh doesn't get marked invalid on replacing a single value.
				_voxels = value;
				_meshIsOutdated = true;
			}
		}

		/// <summary>
		/// Marks the current background mesh of this voxel map as outdated.
		/// Use this if you want to re-generate the mesh after changing the voxel data.
		/// </summary>
		public void MarkMeshOutdated()
		{
			_meshIsOutdated = true;
		}

		public void Render(Renderer renderer, Matrix4 matrix)
		{
			// If the mesh is outdated or we don't have vertices at all, we need to (re)generate it
			if (_meshIsOutdated || _vertices == null)
			{
				_vertices = VoxelMapMesher.GenerateMesh(Voxels).ToArray();
				_meshIsOutdated = false;
			}

			renderer.RenderMeshStreaming(ref matrix, _vertices);
		}
	}
}