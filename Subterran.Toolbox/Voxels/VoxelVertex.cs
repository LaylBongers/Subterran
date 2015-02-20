using OpenTK;

namespace Subterran.Toolbox.Voxels
{
	public struct VoxelVertex
	{
		public VoxelVertex(Vector3 position, VoxelSideCorner corner)
			: this()
		{
			Position = position;
			Corner = corner;
		}

		public Vector3 Position { get; set; }
		public VoxelSideCorner Corner { get; set; }
		public VoxelSide Side { get; set; }

		public VoxelVertex Offset(Vector3 offset)
		{
			return new VoxelVertex
			{
				Position = Position + offset,
				Corner = Corner,
				Side = Side
			};
		}

		public VoxelVertex Transform(Matrix4 matrix, VoxelSide newSide)
		{
			return new VoxelVertex
			{
				Position = Vector3.Transform(Position, matrix),
				Corner = Corner,
				Side = newSide
			};
		}
	}
}