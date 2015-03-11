using System;
using Xunit;

namespace Subterran.Tests
{
	[Trait("Type", "Unit")]
	[Trait("Namespace", "Subterran")]
	[Trait("Class", "Subterran.StReflectionTests")]
	public class StReflectionTests
	{
		[Fact]
		public void GetTypeFromName_SingleConstructorType_FindsType()
		{
			// Arrange
			var type = typeof (ValidTestType).FullName;

			// Act
			var value = StReflection.GetTypeFromName(type);

			// Assert
			Assert.Equal(typeof (ValidTestType), value);
		}
		
		[Fact]
		public void GetTypeFromName_MissingType_ThrowsException()
		{
			// Arrange
			const string type = "Subterran.Tests.StReflectionTests+InvalidTestType";

			// Act & Assert
			Assert.Throws<InvalidOperationException>(() => StReflection.GetTypeFromName(type));
		}

		private sealed class ValidTestType
		{
		}
	}
}