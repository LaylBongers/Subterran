using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using OpenTK;
using Subterran;
using Subterran.Rendering;
using Subterran.Rendering.Vertices;

namespace ComponentGallery
{
	internal static class ModelLoader
	{
		public static ColoredVertex[] Load(string path)
		{
			var lines = File.ReadAllLines(path);

			// Set up lists to load data into
			var positions = new List<Vector3>();
			var indices = new List<int>();

			// Parse all the lines into data
			foreach (var line in lines)
			{
				LoadLine(line, positions, indices);
			}

			// Now that we've got a set of positions and indices we need to flatten it into vertices.
			// We probably should support indices instead on the engine side but this is just an example.
			var random = new Random();
			var positionsArray = positions.ToArray();
			var vertices = indices.Select(i => new ColoredVertex
			{
				Position = positionsArray[i - 1],
				Color = new Vector3(random.NextFloat(), random.NextFloat(), random.NextFloat())
			});

			return vertices.ToArray();
		}

		private static void LoadLine(string line, List<Vector3> positions, List<int> indices)
		{
			var data = line.Split(' ');
			var type = data.First();

			switch (type)
			{
				case "v": // Vertex position
					LoadPosition(data, positions);
					break;

				case "f": // Faces
					LoadFace(data, indices);
					break;
			}
			// We're ignoring anything that's not any of those
		}

		private static void LoadPosition(IEnumerable<string> data, List<Vector3> positions)
		{
			var values = data
				.Skip(1)
				.Select(d => float.Parse(d, CultureInfo.InvariantCulture))
				.ToArray();
			positions.Add(new Vector3(values[0], values[1], values[2]));
		}

		private static void LoadFace(IEnumerable<string> data, List<int> indices)
		{
			var values = data
				.Skip(1)
				.SelectMany(v => v
					.Split(new[] {'/'}, StringSplitOptions.RemoveEmptyEntries)
					.Take(1)
					.Select(pos => int.Parse(pos, CultureInfo.InvariantCulture)));
			indices.AddRange(values);
		}
	}
}