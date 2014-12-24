
namespace Subterran
{
	/// <summary>
	///		Stores an ordered pair of integers, which specify an X and Y position in world units.
	/// </summary>
	public struct WorldPosition
	{
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
	}
	
	/// <summary>
	///		Stores an ordered pair of integers, which specify an X and Y distance in world units.
	/// </summary>
	public struct WorldDistance
	{
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
	}
	
	/// <summary>
	///		Stores an ordered pair of integers, which specify an X and Y size in world units.
	/// </summary>
	public struct WorldSize
	{
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
	}
	public struct WorldRotation
	{
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

	}
}