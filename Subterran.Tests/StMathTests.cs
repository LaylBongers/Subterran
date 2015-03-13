using System;
using Xunit;

namespace Subterran.Tests
{
	[Trait("Type", "Unit")]
	[Trait("Namespace", "Subterran")]
	[Trait("Class", "Subterran.StMathTests")]
	public class StMathTests
	{
		public static object[] SmallBigTimeSpans =
		{
			new object[] {TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(2)},
			new object[] {TimeSpan.FromSeconds(0.10), TimeSpan.FromSeconds(0.16)},
			new object[] {TimeSpan.FromSeconds(4), TimeSpan.FromSeconds(9)},
			new object[] {TimeSpan.FromSeconds(-2), TimeSpan.FromSeconds(-1)}
		};

		public static object[] SignedAddValues =
		{
			new object[] {5, 2, 7},
			new object[] {-6, 2, -8}
		};

		[Theory, MemberData("SmallBigTimeSpans")]
		public void Min_NormalTimeSpans_ReturnsSmallest(TimeSpan small, TimeSpan big)
		{
			// Act
			var result1 = StMath.Min(small, big);
			var result2 = StMath.Min(big, small);

			// Assert
			Assert.Equal(small, result1);
			Assert.Equal(small, result2);
		}

		[Theory, MemberData("SmallBigTimeSpans")]
		public void Max_NormalTimeSpans_ReturnsBiggest(TimeSpan small, TimeSpan big)
		{
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

		[Theory, MemberData("SignedAddValues")]
		public void SignedAdd_VariedData_AddsInSignDirection(int value, int addition, int expected)
		{
			// Act
			var result = StMath.AddSigned(value, addition);

			// Assert
			Assert.Equal(expected, result, 5);
		}
	}
}