using OpenTK;

namespace Subterran
{
	public static class StVector
	{
		public static float GetX(Vector3 value)
		{
			return value.X;
		}

		public static float GetY(Vector3 value)
		{
			return value.Y;
		}

		public static float GetZ(Vector3 value)
		{
			return value.Z;
		}

		public static void SetX(ref Vector3 output, float value)
		{
			output.X = value;
		}

		public static void SetY(ref Vector3 output, float value)
		{
			output.Y = value;
		}

		public static void SetZ(ref Vector3 output, float value)
		{
			output.Z = value;
		}

		public delegate float AxisGetFunc(Vector3 value);

		public delegate void AxisSetAction(ref Vector3 output, float value);
	}
}