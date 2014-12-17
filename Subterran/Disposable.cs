using System;

namespace Subterran
{
	public abstract class Disposable : IDisposable
	{
		public void Dispose()
		{
			Dispose(true);
		}

		~Disposable()
		{
			Dispose(false);
		}

		protected abstract void Dispose(bool managed);
	}
}