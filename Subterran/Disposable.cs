using System;

namespace Subterran
{
	public abstract class Disposable : IDisposable
	{
		public bool IsDisposed { get; private set; }

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		~Disposable()
		{
			Dispose(false);
		}

		protected virtual void Dispose(bool managed)
		{
			if (managed)
			{
				IsDisposed = true;
			}
		}
	}
}