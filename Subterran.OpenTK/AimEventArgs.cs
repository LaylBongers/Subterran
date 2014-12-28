using System;

namespace Subterran.OpenTK
{
	public sealed class AimEventArgs : EventArgs
	{
		public AimEventArgs(int xDelta, int yDelta)
		{
			Delta = new ScreenDistance(xDelta, yDelta);
			XDelta = xDelta;
			YDelta = yDelta;
		}

		public ScreenDistance Delta { get; private set; }
		public int XDelta { get; private set; }
		public int YDelta { get; private set; }
	}
}