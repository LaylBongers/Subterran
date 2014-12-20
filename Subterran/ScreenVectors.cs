
namespace Subterran
{
	public struct ScreenPosition
	{
		private readonly int _x;
		private readonly int _y;

		public ScreenPosition(int x, int y)
		{
			_x = x;
			_y = y;
		}

		public int X { get { return _x; } }
		public int Y { get { return _y; } }

		public static ScreenPosition operator +(ScreenPosition left, ScreenDistance right)
		{
			return new ScreenPosition(left.X + right.X, left.Y + right.Y);
		}
		public static ScreenPosition operator +(ScreenPosition left, ScreenSize right)
		{
			return new ScreenPosition(left.X + right.X, left.Y + right.Y);
		}
	}
	public struct ScreenDistance
	{
		private readonly int _x;
		private readonly int _y;

		public ScreenDistance(int x, int y)
		{
			_x = x;
			_y = y;
		}

		public int X { get { return _x; } }
		public int Y { get { return _y; } }

		public static ScreenDistance operator +(ScreenDistance left, ScreenDistance right)
		{
			return new ScreenDistance(left.X + right.X, left.Y + right.Y);
		}
	}
	public struct ScreenSize
	{
		private readonly int _x;
		private readonly int _y;

		public ScreenSize(int x, int y)
		{
			_x = x;
			_y = y;
		}

		public int X { get { return _x; } }
		public int Y { get { return _y; } }

		public static ScreenSize operator +(ScreenSize left, ScreenSize right)
		{
			return new ScreenSize(left.X + right.X, left.Y + right.Y);
		}
	}
}