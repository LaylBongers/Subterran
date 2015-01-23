using System;
using System.Drawing;
using OpenTK;
using SharpNoise.Modules;
using Subterran;
using Subterran.Rendering;
using Subterran.Toolbox;
using Subterran.Toolbox.Voxels;

namespace ComponentGallery
{
	internal class VoxelWavesGeneratorComponent : EntityComponent, IInitializable, IUpdatable, IRenderablePreparer
	{
		private static readonly Random Random = new Random();
		private readonly Simplex _simplex = new Simplex {Frequency = 0.01};
		private TimeSpan _time;
		private VoxelMapComponent<ColoredVoxel> _voxelMap;
		private ColoredVoxel[,,] _voxels;

		public VoxelWavesGeneratorComponent()
		{
			Size = 10;
			Height = 6;
		}

		public int Size { get; set; }
		public int Height { get; set; }

		public void Initialize()
		{
			_voxelMap = Entity.GetComponent<VoxelMapComponent<ColoredVoxel>>();

			if (_voxelMap == null)
				throw new InvalidOperationException("This component requires a VoxelMapComponent!");

			_voxels = GenerateColoredVoxelMap();
		}

		public void PrepareRender()
		{
			GenerateWaterHeights(_time.TotalSeconds);
			_voxelMap.Voxels = _voxels;
		}

		public void Update(TimeSpan elapsed)
		{
			_time += elapsed;
		}

		private ColoredVoxel[,,] GenerateColoredVoxelMap()
		{
			var random = new Random(5);
			var voxels = new ColoredVoxel[Size, Height + 1, Size];

			for (var x = 0; x < Size; x++)
			{
				for (var z = 0; z < Size; z++)
				{
					var pillarColor = RandomizeColor(random, 10, Color.RoyalBlue);
					for (var y = 0; y < Height + 1; y++)
					{
						voxels[x, y, z].Color = pillarColor;
					}
				}
			}

			return voxels;
		}

		private void GenerateWaterHeights(double time)
		{
			for (var x = 0; x < Size; x++)
			{
				for (var z = 0; z < Size; z++)
				{
					var rangedNoise = _simplex.GetValue(x, time*10, z)*0.5 + 0.5;
					var rawPillarHeight = (rangedNoise*(Height - 1)) + 1;
					var pillarHeight = StMath.Range((int) rawPillarHeight, 1, Height);

					for (var y = 0; y < Height + 1; y++)
					{
						_voxels[x, y, z].IsSolid = y < pillarHeight;
					}
				}
			}
		}

		private static Vector3 RandomizeColor(Random random, int randomness, Color color)
		{
			return new Vector3(
				StMath.NormalizeColor(
					StMath.Range(color.R + random.Next(-randomness, randomness),
						Byte.MinValue, Byte.MaxValue)),
				StMath.NormalizeColor(
					StMath.Range(color.G + random.Next(-randomness, randomness),
						Byte.MinValue, Byte.MaxValue)),
				StMath.NormalizeColor(
					StMath.Range(color.B + random.Next(-randomness, randomness),
						Byte.MinValue, Byte.MaxValue)));
		}
	}
}