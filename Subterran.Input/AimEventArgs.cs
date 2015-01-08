using System;
using OpenTK;

namespace Subterran.Input
{
	public sealed class AimEventArgs : EventArgs
	{
		public AimEventArgs(int xDelta, int yDelta)
		{
			Delta = new Vector2(xDelta, yDelta);
			XDelta = xDelta;
			YDelta = yDelta;
		}

		public Vector2 Delta { get; private set; }
		public int XDelta { get; private set; }
		public int YDelta { get; private set; }
	}
}