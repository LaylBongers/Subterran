using System;

namespace Subterran
{
	// ReSharper disable InconsistentNaming
	public struct Vector3i
		// ReSharper restore InconsistentNaming
		: IEquatable<Vector3i>
	{
		public Vector3i(int value)
			: this()
		{
			X = value;
			Y = value;
			Z = value;
		}

		public Vector3i(int x, int y, int z)
			: this()
		{
			X = x;
			Y = y;
			Z = z;
		}

		public bool Equals(Vector3i other)
		{
			return X == other.X && Y == other.Y && Z == other.Z;
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			return obj is Vector3i && Equals((Vector3i) obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				var hashCode = X;
				hashCode = (hashCode*397) ^ Y;
				hashCode = (hashCode*397) ^ Z;
				return hashCode;
			}
		}

		public static bool operator ==(Vector3i left, Vector3i right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(Vector3i left, Vector3i right)
		{
			return !left.Equals(right);
		}

		public int X { get; set; }
		public int Y { get; set; }
		public int Z { get; set; }
	}
}