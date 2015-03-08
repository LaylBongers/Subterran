using Xunit;

namespace Subterran.Tests
{
	[Trait("Type", "Unit")]
	[Trait("Namespace", "Subterran")]
	[Trait("Class", "Subterran.Disposable")]
	public class DisposableTests
	{
		[Fact]
		public void Dispose_CallsInternalDispose()
		{
			// Arrange
			var disposable = new DisposableSpy();

			// Act
			disposable.Dispose();

			// Assert
			Assert.True(disposable.WasDisposedCorrectly);
		}

		[Fact]
		public void Dispose_SetsIsDisposedTrue()
		{
			// Arrange
			var disposable = new DisposableSpy();

			// Act
			disposable.Dispose();

			// Assert
			Assert.True(disposable.IsDisposed);
		}

		private class DisposableSpy : Disposable
		{
			public bool WasDisposedCorrectly { get; set; }

			protected override void Dispose(bool managed)
			{
				if (managed)
				{
					WasDisposedCorrectly = true;
				}

				base.Dispose(managed);
			}
		}
	}
}