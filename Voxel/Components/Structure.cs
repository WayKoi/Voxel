using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;
using Piles.DataFiles;
using Voxel.Structs;

namespace Voxel.Components {
	internal class Structure {
		private List<StructCube> _cubes = new List<StructCube>();
		private Dictionary<int, int> _typelookup = new Dictionary<int, int>();
		private Dictionary<int, LightDef> _lightlookup = new Dictionary<int, LightDef>();
		private int _typecount = 0, _lightcount = 0;

		private int _height = 0, _width = 0, _depth = 0;

		public int Height {
			get { return _height; }
		}

		public int Width {
			get { return _width; }
		}

		public int Depth {
			get { return _depth; }
		}

		public Structure(string filename) { 
			Load(filename);
		}

		public List<StructCube> Place (int x, int y, int z) {
			List<StructCube> cubes = new List<StructCube>();

			foreach (StructCube cube in _cubes) {
				int type = cube.CubeType == StructCubeType.Cube ? _typelookup[cube.Type] : cube.Type;

				cubes.Add(new StructCube(
					type,
					new Vector3i(
						cube.Position.X + x,
						cube.Position.Y + y,
						cube.Position.Z + z
					),
					cube.CubeType,
					cube.Light
				));
			}

			return cubes;
		}

		private void Load(string filename) {
			List<StructCube?> loaded = DataFile.Parse(filename, Parse);

			foreach (StructCube? cube in loaded) {
				if (cube == null) { continue; }
				_cubes.Add((StructCube) cube);

				if (((StructCube) cube).Position.X > _width) {
					_width = (int) ((StructCube) cube).Position.X;
				}

				if (((StructCube) cube).Position.Y > _height) {
					_height = (int) ((StructCube) cube).Position.Y;
				}

				if (((StructCube) cube).Position.Z > _depth) {
					_depth = (int) ((StructCube) cube).Position.Z;
				}
			}
		}

		private StructCube? Parse (string[] parts) {
			if (parts.Length < 2) { return null; }

			switch (parts[0]) {
				case "def":
					Define(parts);
					break;
				case "c":
					return new StructCube(
						int.Parse(parts[1]),
						new Vector3i(
							int.Parse(parts[2]),
							int.Parse(parts[3]),
							int.Parse(parts[4])
						)
					);
				case "l":
					return new StructCube(
						int.Parse(parts[1]),
						new Vector3i(
							int.Parse(parts[2]),
							int.Parse(parts[3]),
							int.Parse(parts[4])
						),
						StructCubeType.Light,
						_lightlookup[int.Parse(parts[1])]
					);
			}

			return null;
		}

		private void Define (string[] parts) {
			switch (parts[1]) {
				case "static":
					int type = Cube.AddType(new StaticCubeType(
						new Vector3(
							float.Parse(parts[2]),
							float.Parse(parts[3]),
							float.Parse(parts[4])
						),
						float.Parse(parts[5]),
						float.Parse(parts[6])
					));

					_typelookup.Add(_typecount, type);
					_typecount++;

					break;
				case "light":
					Vector3 colour = new Vector3(
						float.Parse(parts[2]),
						float.Parse(parts[3]),
						float.Parse(parts[4])
					);

					Light light = new Light(
						new Vector3(0),
						colour * 0.75f,
						colour
					);

					_lightlookup.Add(_lightcount, new LightDef(float.Parse(parts[5]), light));
					_lightcount++;

					break;
			}
		}

	}

	internal struct StructCube {
		public StructCubeType CubeType;
		public Vector3i Position;
		public int Type;
		public LightDef? Light;

		public StructCube (int type, Vector3i position, StructCubeType cubetype = StructCubeType.Cube, LightDef? light = null) {
			Position = position;
			Type = type;
			CubeType = cubetype;
			Light = light;
		}
	}

	internal struct LightDef {
		public float Distance;
		public Light Light;

		public LightDef (float distance, Light light) {
			Distance = distance;
			Light = light;
		}
	}

	internal enum StructCubeType {
		Cube,
		Light
	}
}
