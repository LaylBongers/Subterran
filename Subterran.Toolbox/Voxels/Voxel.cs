using System.Linq;
using OpenTK;

namespace Subterran.Toolbox.Voxels
{
	public struct Voxel
	{
		public bool IsSolid { get; set; }
		public Vector3 Color { get; set; }

		public static Vector3[] CreateMesh()
		{
			var frontSquare = new[]
			{
				new Vector3(0, 1, 0), // Left Top
				new Vector3(0, 0, 0), // Left Bottom
				new Vector3(1, 0, 0), // Right Bottom

				new Vector3(0, 1, 0), // Left Top
				new Vector3(1, 0, 0), // Right Bottom
				new Vector3(1, 1, 0) // Right Top
			};

			var vertices = frontSquare.ToList();

			var leftVoxelMatrix =
				Matrix4.CreateRotationY(-0.25f*StMath.Tau)*
				Matrix4.CreateTranslation(0, 0, -1);
			vertices.AddRange(frontSquare.Transform(leftVoxelMatrix));

			var rightVoxelMatrix =
				Matrix4.CreateRotationY(0.25f*StMath.Tau)*
				Matrix4.CreateTranslation(1, 0, 0);
			vertices.AddRange(frontSquare.Transform(rightVoxelMatrix));

			var backVoxelMatrix =
				Matrix4.CreateRotationY(0.5f*StMath.Tau)*
				Matrix4.CreateTranslation(1, 0, -1);
			vertices.AddRange(frontSquare.Transform(backVoxelMatrix));

			var topVoxelMatrix =
				Matrix4.CreateRotationX(-0.25f*StMath.Tau)*
				Matrix4.CreateTranslation(0, 1, 0);
			vertices.AddRange(frontSquare.Transform(topVoxelMatrix));

			var bottomVoxelMatrix =
				Matrix4.CreateRotationX(0.25f * StMath.Tau) *
				Matrix4.CreateTranslation(0, 0, -1);
			vertices.AddRange(frontSquare.Transform(bottomVoxelMatrix));

			return vertices.ToArray();
		}
	}
}