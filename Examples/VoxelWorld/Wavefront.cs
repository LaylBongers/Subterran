using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using OpenTK;
using Subterran.Toolbox.Materials;

namespace VoxelWorld
{
	public static class Wavefront
	{
		private static readonly Vector2 OpenGlUvCorrection = new Vector2(1, -1);

		public static WavefrontFile LoadObj(string path)
		{
			// We will have to look in files relative to the directory of the file
			var file = new FileInfo(path);
			var directory = file.Directory;
			Debug.Assert(directory != null);

			// Start by tokenizing the file into lines and space-separated tokens
			var lines = TokenizeFile(file);

			// Find all mtllib statements in the obj file and load them into material objects
			var materials = lines
				.Where(l => l[0] == "mtllib")
				.SelectMany(s =>
				{
					var mtlFile = directory.GetFiles().First(f => f.Name == s[1]);
					return LoadMtl(mtlFile.FullName);
				})
				.ToDictionary(m => m.Name);

			// Define some temporary stuff for parsing
			var outFile = new WavefrontFile();
			var vPositions = new List<Vector3>();
			var vUvs = new List<Vector2>();
			var currentMesh = new List<TexturedVertex>();

			// Temp values for face group state
			WavefrontModel model = null;

			// This is the actual parser
			foreach (var line in lines)
			{
				switch (line[0])
				{
					case "v": // Define a new vertex position
						vPositions.Add(ParseValueVector3(line));
						break;

					case "vt": // Define a new vertex texture uv
						vUvs.Add(ParseValueVector2(line));
						break;

					case "f": // Define a new face for the current smoothing group
						currentMesh.AddRange(ParseFaceValue(line, vPositions, vUvs));
						break;

					case "usemtl": // Set the material we're going to use
						SmoothingGroupChange(outFile, ref model, currentMesh);
						model.Material = materials[line[1]];
						break;

					default:
						// Ignore anything we don't know
						break;
				}
			}

			// Do one last smoothing group check to flush a remaining model to the output
			SmoothingGroupChange(outFile, ref model, currentMesh);

			return outFile;
		}

		private static void SmoothingGroupChange(WavefrontFile file, ref WavefrontModel model,
			List<TexturedVertex> currentMesh)
		{
			// This method checks if we need to go to a new smoothing group or if we can continue the current one

			// If we haven't started a model yet at all
			if (model == null)
			{
				model = new WavefrontModel();
				currentMesh.Clear(); // < just to be sure
				return;
			}

			// If we have started a model but no vertices have been added yet, we don't need a new model yet yet
			if (currentMesh.Count == 0)
			{
				return;
			}

			// If we have vertices, we need a new model, so add the old one to the file and start a new one
			model.Mesh = currentMesh.ToArray();
			file.Models.Add(model);
			model = new WavefrontModel();
			currentMesh.Clear();
		}

		private static IEnumerable<TexturedVertex> ParseFaceValue(string[] statement, List<Vector3> vPositions,
			List<Vector2> vUvs)
		{
			// We're skipping [0] because that's not part of the value
			foreach (var part in statement.Skip(1))
			{
				var values = part.Split('/')
					.Select(v => int.Parse(v, CultureInfo.InvariantCulture))
					.ToArray();

				// Indices start at 1 for who knows what reason
				var vertex = new TexturedVertex();
				vertex.Position = vPositions[values[0] - 1];
				vertex.TexCoord = vUvs[values[2] - 1]*OpenGlUvCorrection;
				yield return vertex;
			}
		}

		private static Vector3 ParseValueVector3(string[] statement)
		{
			// We're skipping [0] because that's not part of the value
			var vector = new Vector3();
			vector.X = float.Parse(statement[1], CultureInfo.InvariantCulture);
			vector.Y = float.Parse(statement[2], CultureInfo.InvariantCulture);
			vector.Z = float.Parse(statement[3], CultureInfo.InvariantCulture);
			return vector;
		}

		private static Vector2 ParseValueVector2(string[] statement)
		{
			// We're skipping [0] because that's not part of the value
			var vector = new Vector2();
			vector.X = float.Parse(statement[1], CultureInfo.InvariantCulture);
			vector.Y = float.Parse(statement[2], CultureInfo.InvariantCulture);
			return vector;
		}

		public static IEnumerable<WavefrontMaterial> LoadMtl(string path)
		{
			// We will have to look in files relative to the directory of the file
			var file = new FileInfo(path);
			var directory = file.Directory;
			Debug.Assert(directory != null);

			var lines = TokenizeFile(file);

			// This is the actual parser
			WavefrontMaterial currentMaterial = null;
			foreach (var line in lines)
			{
				switch (line[0])
				{
					case "newmtl": // Create a new material
						// If we already had a material, we're now making a new one so return the old one
						if (currentMaterial != null)
							yield return currentMaterial;

						currentMaterial = new WavefrontMaterial();
						currentMaterial.Name = line[1];
						break;

					case "map_Kd": // Set the Diffuse Texture
						if (currentMaterial == null)
							throw new InvalidOperationException("Cannot set texture before material is created.");

						var texture = directory.GetFiles().First(f => f.Name == line[1]);
						currentMaterial.Texture = texture;
						break;

					default:
						// Ignore anything we don't know
						break;
				}
			}

			// If we still have a material, return it now
			if (currentMaterial != null)
				yield return currentMaterial;
		}

		private static List<string[]> TokenizeFile(FileInfo file)
		{
			return File.ReadAllLines(file.FullName)
				.Select(RemoveComment)
				.Select(l => l.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries))
				.Where(l => l.Length != 0)
				.ToList();
		}

		private static string RemoveComment(string line)
		{
			var commentStart = line.IndexOf("#", StringComparison.Ordinal);

			// In this situation, no comment found
			if (commentStart == -1)
				return line;

			return line.Substring(0, commentStart);
		}
	}

	public sealed class WavefrontFile
	{
		public Collection<WavefrontModel> Models { get; } = new Collection<WavefrontModel>();
	}

	public sealed class WavefrontModel
	{
		public TexturedVertex[] Mesh { get; set; }
		public WavefrontMaterial Material { get; set; }
	}

	public sealed class WavefrontMaterial
	{
		public string Name { get; set; }
		public FileInfo Texture { get; set; }
	}
}