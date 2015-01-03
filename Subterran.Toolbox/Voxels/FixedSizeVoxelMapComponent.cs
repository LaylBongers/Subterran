using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK;
using Subterran.OpenTK;

namespace Subterran.Toolbox.Voxels
{
	public class FixedSizeVoxelMapComponent : RenderEntityComponent
	{
		private bool _meshIsOutdated;
		private ColoredVertex[] _vertices;
		private Voxel[][][] _voxels;

		public FixedSizeVoxelMapComponent()
			: this(0, 0, 0)
		{
		}

		public FixedSizeVoxelMapComponent(int width, int height, int depth)
		{
			Voxels = StArray.CreateJagged<Voxel[][][]>(width, height, depth);
		}

		public Voxel[][][] Voxels
		{
			get { return _voxels; }
			set
			{
				_voxels = value;
				_meshIsOutdated = true;
			}
		}

		public override void Update(TimeSpan elapsed)
		{
			// We only need to update if the mesh is outdated
			if (!_meshIsOutdated)
				return;

			var random = new Random();
			var vertices = new List<ColoredVertex>();

			for (var x = 0; x < Voxels.Length; x++)
			{
				for (var y = 0; y < Voxels[x].Length; y++)
				{
					for (var z = 0; z < Voxels[x][y].Length; z++)
					{
						if (!Voxels[x][y][z].IsSolid)
							continue;

						vertices.AddRange(Voxel.CreateMesh(
							x <= 0 || !Voxels[x - 1][y][z].IsSolid,
							x >= Voxels.Length - 1 || !Voxels[x + 1][y][z].IsSolid,

							y <= 0 || !Voxels[x][y - 1][z].IsSolid,
							y >= Voxels[x].Length - 1 || !Voxels[x][y + 1][z].IsSolid,
							
							z <= 0 || !Voxels[x][y][z - 1].IsSolid,
							z >= Voxels[x][y].Length - 1 || !Voxels[x][y][z + 1].IsSolid)

							.Transform(Matrix4.CreateTranslation(x, y, z))
							.Select(v => new ColoredVertex
							{
								Position = v,
								Color = Voxels[x][y][z].Color
							}));
					}
				}
			}

			_vertices = vertices.ToArray();
			_meshIsOutdated = false;
		}

		public override void Render(Renderer renderer, Matrix4 matrix)
		{
			// We can't render if the mesh hasn't been created yet
			if (_vertices == null)
				return;

			renderer.RenderMeshStreaming(ref matrix, _vertices);
		}
	}
}