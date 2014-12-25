
using System;

namespace Subterran
{
	/// <summary>
	///		Stores an ordered pair of integers, which specify an X and Y position in pixels.
	/// </summary>
	public struct ScreenPosition : IEquatable<ScreenPosition>
	{
		public static readonly ScreenPosition Zero = new ScreenPosition();

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
		
		public bool Equals(ScreenPosition other)
		{
			return _x.Equals(other._x) && _y.Equals(other._y);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			return obj is ScreenPosition && Equals((ScreenPosition) obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return (_x*397) ^ _y;
			}
		}

		public static bool operator ==(ScreenPosition left, ScreenPosition right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(ScreenPosition left, ScreenPosition right)
		{
			return !left.Equals(right);
		}
	}
	
	/// <summary>
	///		Stores an ordered pair of integers, which specify an X and Y distance in pixels.
	/// </summary>
	public struct ScreenDistance : IEquatable<ScreenDistance>
	{
		public static readonly ScreenDistance Zero = new ScreenDistance();

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
		
		public bool Equals(ScreenDistance other)
		{
			return _x.Equals(other._x) && _y.Equals(other._y);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			return obj is ScreenDistance && Equals((ScreenDistance) obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return (_x*397) ^ _y;
			}
		}

		public static bool operator ==(ScreenDistance left, ScreenDistance right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(ScreenDistance left, ScreenDistance right)
		{
			return !left.Equals(right);
		}
	}
	
	/// <summary>
	///		Stores an ordered pair of integers, which specify an X and Y size in pixels.
	/// </summary>
	public struct ScreenSize : IEquatable<ScreenSize>
	{
		public static readonly ScreenSize Zero = new ScreenSize();

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
		
		public bool Equals(ScreenSize other)
		{
			return _x.Equals(other._x) && _y.Equals(other._y);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			return obj is ScreenSize && Equals((ScreenSize) obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return (_x*397) ^ _y;
			}
		}

		public static bool operator ==(ScreenSize left, ScreenSize right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(ScreenSize left, ScreenSize right)
		{
			return !left.Equals(right);
		}
	}
}