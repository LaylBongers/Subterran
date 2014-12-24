using System;
using Xunit;

namespace Subterran.Tests
{
	public class StMathTests
	{
		[Fact]
		public void Min_NormalTimeSpans_ReturnsSmallest()
		{
			// Arrange
			var small = TimeSpan.FromSeconds(1);
			var big = TimeSpan.FromSeconds(2);

			// Act
			var result = StMath.Min(small, big);

			// Assert
			Assert.Equal(small, result);
		}

		[Fact]
		public void Max_NormalTimeSpans_ReturnsBiggest()
		{
			// Arrange
			var small = TimeSpan.FromSeconds(1);
			var big = TimeSpan.FromSeconds(2);

			// Act
			var result = StMath.Max(small, big);

			// Assert
			Assert.Equal(big, result);
		}
	}
}