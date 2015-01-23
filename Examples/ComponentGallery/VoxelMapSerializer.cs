using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using OpenTK;
using Subterran;
using Subterran.Toolbox.Voxels;

namespace ComponentGallery
{
	public static class VoxelMapSerializer
	{
		private static readonly Random Random = new Random();

		public static ColoredVoxel[,,] Load(string path)
		{
			var file = File.OpenRead(path);
			var reader = new StreamReader(file);

			// Read in the size and create a new array for it
			var voxels = LoadSizes(reader);

			// Each block in the file is a different Y slice
			for (var y = 0; y < voxels.GetLength(1); y++)
			{
				LoadSlice(reader, voxels, y);
			}

			return voxels;
		}

		private static ColoredVoxel[,,] LoadSizes(StreamReader dataSource)
		{
			var sizesLine = dataSource.ReadLine();
			Debug.Assert(sizesLine != null);
			var sizes = sizesLine
				.Split(new[] {',', ' '}, StringSplitOptions.RemoveEmptyEntries)
				.Select(int.Parse)
				.ToArray();

			return new ColoredVoxel[sizes[0], sizes[1], sizes[2]];
		}

		private static void LoadSlice(StreamReader dataSource, ColoredVoxel[,,] voxels, int y)
		{
			// Skip the divider line
			dataSource.ReadLine();

			// Each line is a different Z
			for (var z = 0; z < voxels.GetLength(2); z++)
			{
				var line = dataSource.ReadLine();
				Debug.Assert(line != null);
				Debug.Assert(line.Length == voxels.GetLength(0));

				// Each character is a different X
				for (var x = 0; x < voxels.GetLength(0); x++)
				{
					LoadVoxel(voxels, x, y, z, line[x]);
				}
			}
		}

		private static void LoadVoxel(ColoredVoxel[,,] voxels, int x, int y, int z, char ch)
		{
			var color = GetColorFor(ch);

			if (color != Color.Empty)
			{
				voxels[x, y, z].IsSolid = true;
				voxels[x, y, z].Color = RandomizeColor(10, color);
			}
			else
			{
				voxels[x, y, z].IsSolid = false;
			}
		}

		private static Color GetColorFor(char ch)
		{
			switch (ch)
			{
				case '1':
					return Color.SaddleBrown;
				case '2':
					return Color.Green;
				default:
					return Color.Empty;
			}
		}

		private static Vector3 RandomizeColor(int randomness, Color color)
		{
			return new Vector3(
				StMath.NormalizeColor(
					StMath.Range(color.R + Random.Next(-randomness, randomness),
						Byte.MinValue, Byte.MaxValue)),
				StMath.NormalizeColor(
					StMath.Range(color.G + Random.Next(-randomness, randomness),
						Byte.MinValue, Byte.MaxValue)),
				StMath.NormalizeColor(
					StMath.Range(color.B + Random.Next(-randomness, randomness),
						Byte.MinValue, Byte.MaxValue)));
		}
	}
}