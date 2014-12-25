
using System;

namespace Subterran
{
	/// <summary>
	///		Stores an ordered pair of integers, which specify an X and Y position in world units.
	/// </summary>
	public struct WorldPosition : IEquatable<WorldPosition>
	{
		public static readonly WorldPosition Zero = new WorldPosition();

		private readonly float _x;
		private readonly float _y;
		private readonly float _z;

		public WorldPosition(float x, float y, float z)
		{
			_x = x;
			_y = y;
			_z = z;
		}

		public float X { get { return _x; } }
		public float Y { get { return _y; } }
		public float Z { get { return _z; } }

		// Scales the WorldPosition to the given amount.
		public static WorldPosition operator *(WorldPosition left, float right)
		{
			return new WorldPosition(left.X * right, left.Y * right, left.Z * right);
		} 

		public static WorldPosition operator +(WorldPosition left, WorldDistance right)
		{
			return new WorldPosition(left.X + right.X, left.Y + right.Y, left.Z + right.Z);
		}
		public static WorldPosition operator +(WorldPosition left, WorldSize right)
		{
			return new WorldPosition(left.X + right.X, left.Y + right.Y, left.Z + right.Z);
		}
		
		public bool Equals(WorldPosition other)
		{
			return _x.Equals(other._x) && _y.Equals(other._y) && _z.Equals(other._z);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			return obj is WorldPosition && Equals((WorldPosition) obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				int hashCode = _x.GetHashCode();
				hashCode = (hashCode*397) ^ _y.GetHashCode();
				hashCode = (hashCode*397) ^ _z.GetHashCode();
				return hashCode;
			}
		}

		public static bool operator ==(WorldPosition left, WorldPosition right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(WorldPosition left, WorldPosition right)
		{
			return !left.Equals(right);
		}
	}
	
	/// <summary>
	///		Stores an ordered pair of integers, which specify an X and Y distance in world units.
	/// </summary>
	public struct WorldDistance : IEquatable<WorldDistance>
	{
		public static readonly WorldDistance Zero = new WorldDistance();

		private readonly float _x;
		private readonly float _y;
		private readonly float _z;

		public WorldDistance(float x, float y, float z)
		{
			_x = x;
			_y = y;
			_z = z;
		}

		public float X { get { return _x; } }
		public float Y { get { return _y; } }
		public float Z { get { return _z; } }

		// Scales the WorldDistance to the given amount.
		public static WorldDistance operator *(WorldDistance left, float right)
		{
			return new WorldDistance(left.X * right, left.Y * right, left.Z * right);
		} 

		public static WorldDistance operator +(WorldDistance left, WorldDistance right)
		{
			return new WorldDistance(left.X + right.X, left.Y + right.Y, left.Z + right.Z);
		}
		
		public bool Equals(WorldDistance other)
		{
			return _x.Equals(other._x) && _y.Equals(other._y) && _z.Equals(other._z);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			return obj is WorldDistance && Equals((WorldDistance) obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				int hashCode = _x.GetHashCode();
				hashCode = (hashCode*397) ^ _y.GetHashCode();
				hashCode = (hashCode*397) ^ _z.GetHashCode();
				return hashCode;
			}
		}

		public static bool operator ==(WorldDistance left, WorldDistance right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(WorldDistance left, WorldDistance right)
		{
			return !left.Equals(right);
		}
	}
	
	/// <summary>
	///		Stores an ordered pair of integers, which specify an X and Y size in world units.
	/// </summary>
	public struct WorldSize : IEquatable<WorldSize>
	{
		public static readonly WorldSize Zero = new WorldSize();

		private readonly float _x;
		private readonly float _y;
		private readonly float _z;

		public WorldSize(float x, float y, float z)
		{
			_x = x;
			_y = y;
			_z = z;
		}

		public float X { get { return _x; } }
		public float Y { get { return _y; } }
		public float Z { get { return _z; } }

		// Scales the WorldSize to the given amount.
		public static WorldSize operator *(WorldSize left, float right)
		{
			return new WorldSize(left.X * right, left.Y * right, left.Z * right);
		} 

		public static WorldSize operator +(WorldSize left, WorldSize right)
		{
			return new WorldSize(left.X + right.X, left.Y + right.Y, left.Z + right.Z);
		}
		
		public bool Equals(WorldSize other)
		{
			return _x.Equals(other._x) && _y.Equals(other._y) && _z.Equals(other._z);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			return obj is WorldSize && Equals((WorldSize) obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				int hashCode = _x.GetHashCode();
				hashCode = (hashCode*397) ^ _y.GetHashCode();
				hashCode = (hashCode*397) ^ _z.GetHashCode();
				return hashCode;
			}
		}

		public static bool operator ==(WorldSize left, WorldSize right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(WorldSize left, WorldSize right)
		{
			return !left.Equals(right);
		}
	}
	public struct WorldRotation : IEquatable<WorldRotation>
	{
		public static readonly WorldRotation Zero = new WorldRotation();

		private readonly float _x;
		private readonly float _y;
		private readonly float _z;

		public WorldRotation(float x, float y, float z)
		{
			_x = x;
			_y = y;
			_z = z;
		}

		public float X { get { return _x; } }
		public float Y { get { return _y; } }
		public float Z { get { return _z; } }

		// Scales the WorldRotation to the given amount.
		public static WorldRotation operator *(WorldRotation left, float right)
		{
			return new WorldRotation(left.X * right, left.Y * right, left.Z * right);
		} 

		public static WorldRotation operator +(WorldRotation left, WorldRotation right)
		{
			return new WorldRotation(left.X + right.X, left.Y + right.Y, left.Z + right.Z);
		}
		
		public bool Equals(WorldRotation other)
		{
			return _x.Equals(other._x) && _y.Equals(other._y) && _z.Equals(other._z);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			return obj is WorldRotation && Equals((WorldRotation) obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				int hashCode = _x.GetHashCode();
				hashCode = (hashCode*397) ^ _y.GetHashCode();
				hashCode = (hashCode*397) ^ _z.GetHashCode();
				return hashCode;
			}
		}

		public static bool operator ==(WorldRotation left, WorldRotation right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(WorldRotation left, WorldRotation right)
		{
			return !left.Equals(right);
		}
	}
}