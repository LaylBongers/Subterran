using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using OpenTK;
using Subterran;

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
				var data = line.Split(' ');
				var type = data.First();

				switch (type)
				{
					case "v": // Vertex position
					{
						var values = data.Skip(1).Select(d => float.Parse(d, CultureInfo.InvariantCulture)).ToArray();
						positions.Add(new Vector3(values[0], values[1], values[2]));
						break;
					}

					case "f": // Faces
					{
						var values = data.Skip(1)
							.SelectMany(v => v
								.Split(new[] {'/'}, StringSplitOptions.RemoveEmptyEntries).Take(1)
								.Select(pos => int.Parse(pos, CultureInfo.InvariantCulture)));
						indices.AddRange(values);
						break;
					}
				}
				// We're ignoring anything that's not any of those
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
	}
}