using System;
using Subterran.Toolbox.Voxels;
using Xunit;

namespace Subterran.Tests.Toolbox.Voxels
{
	public class VoxelMapComponentTests
	{
		[Fact]
		public void PrepareRender_NoMeshGenerator_ThrowsException()
		{
			// Arrange
			var map = new VoxelMapComponent<TestVoxel> {MeshGenerator = null};

			// Act & Assert
			Assert.Throws<InvalidOperationException>(() => map.PrepareRender());
		}

		[Fact]
		public void PrepareRender_WithMeshGenerator_ThrowsNoExceptions()
		{
			// Arrange
			var map = new VoxelMapComponent<TestVoxel> {MeshGenerator = TestVoxel.MeshGenerator};

			// Act & Assert
			map.PrepareRender();
		}
	}

	internal struct TestVoxel
	{
		public static ColoredVertex[] MeshGenerator(TestVoxel[,,] voxels)
		{
			return new ColoredVertex[0];
		}
	}
}