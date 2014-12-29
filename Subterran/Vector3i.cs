using OpenTK;

namespace Subterran
{
	public struct Vector3i
	{
		private readonly int _x;
		private readonly int _y;
		private readonly int _z;

		public Vector3i(int xPos, int yPos, int zPos)
		{
			_x = xPos;
			_y = yPos;
			_z = zPos;
		}

		public int X
		{
			get { return _x; }
		}

		public int Y
		{
			get { return _y; }
		}

		public int Z
		{
			get { return _z; }
		}

		public static Vector3i operator *(Vector3i vector, int scale)
		{
			return Multiply(vector, scale);
		}

		public static Vector3i Multiply(Vector3i vector, int scale)
		{
			return new Vector3i(vector._x*scale, vector._y*scale, vector._z*scale);
		}

		public Vector3 ToVector3()
		{
			return new Vector3(_x, _y, _z);
		}

		#region Equality Functions and Operators

		public override int GetHashCode()
		{
			return _x.GetHashCode() ^ _y.GetHashCode() ^ _z.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			if (!(obj is Vector3i))
				return false;

			return Equals((Vector3i) obj);
		}

		private bool Equals(Vector3i other)
		{
			if (_x != other._x)
				return false;
			if (_y != other._y)
				return false;
			return _z == other._z;
		}

		public static bool operator ==(Vector3i left, Vector3i right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(Vector3i left, Vector3i right)
		{
			return !left.Equals(right);
		}

		#endregion
	}
}