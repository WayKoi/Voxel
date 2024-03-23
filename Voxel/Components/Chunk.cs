using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voxel.Structs;

namespace Voxel.Components {
	internal class Chunk {
		private Vector3 _position;

		private List<Vertex3D> points = new List<Vertex3D>();
		private Cube?[,,] cubes;
		private int _chunksize;
		private float _chunkScale = 1f;

		private int VAO, VBO;

		public Chunk(int x, int y, int z, int chunkSize = 32) {
			_position = new Vector3(x, y, z);

			cubes = new Cube?[chunkSize, chunkSize, chunkSize];

			_chunksize = chunkSize;
		}

		public void Load () {
			VBO = Vertex3D.GenVBO(new Vertex3D[0]);
			VAO = Vertex3D.GenVAO(VBO);
		}

		public void Add (Cube cube, Vector3 Position) {
			cubes[(int) Position.X, (int) Position.Y, (int) Position.Z] = cube;

			Update();
		}

		public void Add(Cube[] adds, Vector3[] Positions) {
			for (int i = 0; i < adds.Length && i < Positions.Length; i++) {
				cubes[(int) Positions[i].X, (int) Positions[i].Y, (int) Positions[i].Z] = adds[i];
			}

			Update();
		}

		private void Update () {
			points.Clear();

			bool[,,] plane = new bool[_chunksize, _chunksize, 6];
			bool[,,] obscure = new bool[_chunksize, _chunksize, 6];

			for (int a = 0; a < _chunksize; a++) {
				for (int b = 0; b < _chunksize; b++) {
					for (int c = 0; c < _chunksize; c++) {
						if (cubes == null) { continue; }
						// going through x plane forwards (rep yz-plane) left face
						Cube? cube = cubes[a, b, c];
						plane[b, c, 0] = cube != null;

						float sa = a * _chunkScale, sb = b * _chunkScale, sc = c * _chunkScale;
						float chunkMax = (_chunksize - 1) * _chunkScale;
						Vector3 scalar = new Vector3(_chunkScale);

						if (cube != null && !obscure[b, c, 0]) {
							// add the points for this square
							Cube point = (Cube) cube;
							points.AddRange(
								Cube.getFace(Face.Left, _position + new Vector3(sa, sb, sc), point.Colour, scalar)
							);
						}

						// going through x plane backwards (rep yz-plane) right face
						cube = cubes[_chunksize - 1 - a, b, c];
						plane[b, c, 1] = cube != null;

						if (cube != null && !obscure[b, c, 1]) {
							// add the points for this square
							Cube point = (Cube) cube;
							points.AddRange(
								Cube.getFace(Face.Right, _position + new Vector3(chunkMax - sa, sb, sc), point.Colour, scalar)
							);
						}


						// going through y plane forwards (rep xz-plane) bottom face
						cube = cubes[b, a, c];
						plane[b, c, 2] = cube != null;

						if (cube != null && !obscure[b, c, 2]) {
							// add the points for this square
							Cube point = (Cube) cube;
							points.AddRange(
								Cube.getFace(Face.Bottom, _position + new Vector3(sb, sa, sc), point.Colour, scalar)
							);
						}

						// going through y plane backwards (rep xz-plane) top face
						cube = cubes[b, _chunksize - 1 - a, c];
						plane[b, c, 3] = cube != null;

						if (cube != null && !obscure[b, c, 3]) {
							// add the points for this square
							Cube point = (Cube) cube;
							points.AddRange(
								Cube.getFace(Face.Top, _position + new Vector3(sb, chunkMax - sa, sc), point.Colour, scalar)
							);
						}


						// going through z plane forwards (rep xy-plane) back face
						cube = cubes[b, c, a];
						plane[b, c, 4] = cube != null;

						if (cube != null && !obscure[b, c, 4]) {
							// add the points for this square
							Cube point = (Cube) cube;
							points.AddRange(
								Cube.getFace(Face.Back, _position + new Vector3(sb, sc, sa), point.Colour, scalar)
							);
						}

						// going through z plane backwards (rep xy-plane) front face
						cube = cubes[b, c, _chunksize - 1 - a];
						plane[b, c, 5] = cube != null;

						if (cube != null && !obscure[b, c, 5]) {
							// add the points for this square
							Cube point = (Cube) cube;
							points.AddRange(
								Cube.getFace(Face.Front, _position + new Vector3(sb, sc, chunkMax - sa), point.Colour, scalar)
							);
						}
					}
				}

				obscure = plane;
				plane = new bool[_chunksize, _chunksize, 6];
			}

			Vertex3D.BufferVertices(VBO, points.ToArray(), BufferUsageHint.StaticDraw);
		}

		public void Render () {
			GL.BindVertexArray(VAO);
			GL.DrawArrays(PrimitiveType.Triangles, 0, points.Count);
			GL.BindVertexArray(0);
		}

	}
}
