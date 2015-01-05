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
		public static Voxel[][][] Load(string path)
		{
			var random = new Random();
			var file = File.OpenRead(path);
			var reader = new StreamReader(file);

			// Read in the size and create a new array for it
			var sizesLine = reader.ReadLine();
			Debug.Assert(sizesLine != null);
			var sizes = sizesLine
				.Split(new[] {',', ' '}, StringSplitOptions.RemoveEmptyEntries)
				.Select(int.Parse)
				.ToArray();
			var voxels = StArray.CreateJagged<Voxel[][][]>(sizes[0], sizes[1], sizes[2]);

			// Each block in the file is a different Y slice
			for (var y = 0; y < voxels[0].Length; y++)
			{
				// Skip the divider line
				reader.ReadLine();

				// Each line is a different Z
				for (var z = 0; z < voxels[0][0].Length; z++)
				{
					var line = reader.ReadLine();
					Debug.Assert(line != null);

					// Each character is a different X
					for (var x = 0; x < voxels.Length; x++)
					{
						Color color;

						switch (line[x])
						{
							case '1':
								color = Color.SaddleBrown;
								break;
							case '2':
								color = Color.Green;
								break;
							default:
								color = Color.Empty;
								break;
						}

						if (color != Color.Empty)
						{
							voxels[x][y][z].IsSolid = true;
							voxels[x][y][z].Color = new Vector3(
								StMath.NormalizeColor(
									StMath.Range(color.R + random.Next(-10, 10),
										Byte.MinValue, Byte.MaxValue)),
								StMath.NormalizeColor(
									StMath.Range(color.G + random.Next(-10, 10),
										Byte.MinValue, Byte.MaxValue)),
								StMath.NormalizeColor(
									StMath.Range(color.B + random.Next(-10, 10),
										Byte.MinValue, Byte.MaxValue)));
						}
						else
						{
							voxels[x][y][z].IsSolid = false;
						}
					}
				}
			}

			return voxels;
		}
	}
}