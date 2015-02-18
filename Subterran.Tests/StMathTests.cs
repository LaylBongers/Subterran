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
			var result1 = StMath.Min(small, big);
			var result2 = StMath.Min(big, small);

			// Assert
			Assert.Equal(small, result1);
			Assert.Equal(small, result2);
		}

		[Fact]
		public void Max_NormalTimeSpans_ReturnsBiggest()
		{
			// Arrange
			var small = TimeSpan.FromSeconds(1);
			var big = TimeSpan.FromSeconds(2);

			// Act
			var result1 = StMath.Max(small, big);
			var result2 = StMath.Max(big, small);

			// Assert
			Assert.Equal(big, result1);
			Assert.Equal(big, result2);
		}

		[Fact]
		public void NormalizeColor_IntegerWithinRange_ReturnsNormalized()
		{
			// Arrange
			const int value = 0xFF/2;
			
			// Act
			var result = StMath.NormalizeColor(value);

			// Assert
			Assert.Equal(0.50, result, 2);
		}

		[Fact]
		public void SignedAdd_FiveAndTwo_ReturnsSeven()
		{
			// Arrange
			const float value = 5, addition = 2, expected = 7;

			// Act
			var result = StMath.AddSigned(value, addition);

			// Assert
			Assert.Equal(expected, result, 5);
		}

		[Fact]
		public void SignedAdd_MinusSixAndTwo_ReturnsMinusEight()
		{
			// Arrange
			const float value = -6, addition = 2, expected = -8;

			// Act
			var result = StMath.AddSigned(value, addition);

			// Assert
			Assert.Equal(expected, result, 5);
		}
	}
}