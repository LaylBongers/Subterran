using System.Collections.Generic;
using OpenTK;

namespace Subterran.Toolbox.Voxels
{
	public struct Voxel
	{
		public bool IsSolid { get; set; }
		public Vector3 Color { get; set; }

		public static Vector3[] CreateMesh(bool left, bool right, bool bottom, bool top, bool back, bool front)
		{
			var square = new[]
			{
				new Vector3(0, 1, 0), // Left Top
				new Vector3(0, 0, 0), // Left Bottom
				new Vector3(1, 0, 0), // Right Bottom

				new Vector3(0, 1, 0), // Left Top
				new Vector3(1, 0, 0), // Right Bottom
				new Vector3(1, 1, 0) // Right Top
			};

			var vertices = new List<Vector3>();

			if (left)
			{
				var leftVoxelMatrix =
					Matrix4.CreateRotationY(-0.25f*StMath.Tau)*
					Matrix4.CreateTranslation(0, 0, 0);
				vertices.AddRange(square.Transform(leftVoxelMatrix));
			}

			if (right)
			{
				var rightVoxelMatrix =
					Matrix4.CreateRotationY(0.25f*StMath.Tau)*
					Matrix4.CreateTranslation(1, 0, 1);
				vertices.AddRange(square.Transform(rightVoxelMatrix));
			}

			if (top)
			{
				var topVoxelMatrix =
					Matrix4.CreateRotationX(-0.25f*StMath.Tau)*
					Matrix4.CreateTranslation(0, 1, 1);
				vertices.AddRange(square.Transform(topVoxelMatrix));
			}

			if (bottom)
			{
				var bottomVoxelMatrix =
					Matrix4.CreateRotationX(0.25f*StMath.Tau)*
					Matrix4.CreateTranslation(0, 0, 0);
				vertices.AddRange(square.Transform(bottomVoxelMatrix));
			}

			if (front)
			{
				var frontVoxelMatrix =
					Matrix4.CreateTranslation(0, 0, 1);
				vertices.AddRange(square.Transform(frontVoxelMatrix));
			}

			if (back)
			{
				var backVoxelMatrix =
					Matrix4.CreateRotationY(0.5f * StMath.Tau) *
					Matrix4.CreateTranslation(1, 0, 0);
				vertices.AddRange(square.Transform(backVoxelMatrix));
			}

			return vertices.ToArray();
		}
	}
}