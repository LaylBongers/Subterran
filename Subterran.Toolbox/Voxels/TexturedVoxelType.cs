using OpenTK;

namespace Subterran.Toolbox.Voxels
{
	public struct TextureLocation
	{
		public TextureLocation(Vector2 start, Vector2 end)
			: this()
		{
			Start = start;
			End = end;
		}

		public Vector2 Start { get; set; }
		public Vector2 End { get; set; }
	}

	public sealed class TexturedVoxelType
	{
		private readonly TextureLocation[] _sides = new TextureLocation[(int) VoxelSide.Count];
		public bool IsTransparent { get; set; }

		public TextureLocation GetSide(VoxelSide side)
		{
			return _sides[(int) side];
		}

		public TexturedVoxelType SetSide(VoxelSide side, TextureLocation location)
		{
			_sides[(int) side] = location;

			return this;
		}

		public TexturedVoxelType SetAllSides(TextureLocation location)
		{
			for (var i = 0; i < _sides.Length; i++)
			{
				_sides[i] = location;
			}

			return this;
		}

		public TexturedVoxelType SetHorizontalSides(TextureLocation location)
		{
			SetSide(VoxelSide.Top, location);
			SetSide(VoxelSide.Bottom, location);

			return this;
		}

		public TexturedVoxelType SetVerticalSides(TextureLocation location)
		{
			SetSide(VoxelSide.West, location);
			SetSide(VoxelSide.East, location);
			SetSide(VoxelSide.South, location);
			SetSide(VoxelSide.North, location);

			return this;
		}

		public TexturedVoxelType SetTransparent()
		{
			IsTransparent = true;
			return this;
		}

		public TexturedVoxelType SetOpaque()
		{
			IsTransparent = false;
			return this;
		}
	}
}