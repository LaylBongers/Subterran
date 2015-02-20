namespace Subterran
{
	// ReSharper disable InconsistentNaming
	public struct Vector3i
		// ReSharper restore InconsistentNaming
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

		public int X { get; set; }
		public int Y { get; set; }
		public int Z { get; set; }
	}
}