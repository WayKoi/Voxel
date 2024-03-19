using OpenTK.Graphics.ES11;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Voxel.Structs;

namespace Voxel.Components {
	internal class Chunk {
		private Vector3 _position;

		private Cube?[,,] cubes;
		private int _chunksize;

		private int VAO, VBO;

		public Chunk(int x, int y, int z, int chunkSize = 32) {
			_position = new Vector3(x, y, z);

			cubes = new Cube?[chunkSize, chunkSize, chunkSize];

			_chunksize = chunkSize;
		}

		public void Add (Cube cube) {
			cubes[(int) cube.Position.X, (int) cube.Position.Y, (int) cube.Position.Z] = cube;

			Update();
		}

		public void Add(Cube[] adds) {
			foreach (Cube cube in adds) {
				cubes[(int) cube.Position.X, (int) cube.Position.Y, (int) cube.Position.Z] = cube;
			}

			Update();
		}

		private void Update () {
			bool[,] plane = new bool[_chunksize, _chunksize];
			bool[,] obscure = new bool[_chunksize, _chunksize];

			List<Vertex3D> points = new List<Vertex3D>();

			for (int x = 0; x < _chunksize; x++) {
				for (int y = 0; y < _chunksize; y++) {
					for (int z = 0; z < _chunksize; z++) {
						if (cubes[x, y, z] == null) { continue; }
						Cube cube = (Cube) cubes[x, y, z];
						
						plane[y, z] = cubes[x, y, z] != null;

						if (plane[y, z] && !obscure[y, z]) {
							// add the points for this square
							points.AddRange(
								new Vertex3D[] {
									new Vertex3D(cube.Position, _position, cube.Colour),
									new Vertex3D(),
									new Vertex3D(),

									new Vertex3D(),
									new Vertex3D(),
									new Vertex3D()
								}
							);
						}
					}
				}

				obscure = plane;
				plane = new bool[_chunksize, _chunksize];
			}
		}

	}
}
