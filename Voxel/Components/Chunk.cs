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

			// create blocks
			bool[,,] accounted = new bool[_chunksize, _chunksize, _chunksize];

			// expand on x then y then z

			// find unaccounted for cube
			// create a block

			List<Block> blocks = new List<Block>();

			for (int x = 0; x < _chunksize; x++) {
				for (int y = 0; y < _chunksize; y++) {
					for (int z = 0; z < _chunksize; z++) {
						if (cubes[x, y, z] == null) {
							accounted[x, y, z] = true;
						}

						if (!accounted[x, y, z]) {
							accounted[x, y, z] = true;
							blocks.Add(CreateBlock(x, y, z, accounted));
						}
					}
				}
			}
			
			// cull faces not needed
			foreach (Block block in blocks) {
				bool cull = true;

				// top face +y
				if (block.Position.Y + block.Size.Y < _chunksize) {
					for (int x = (int) block.Position.X; x < (int) (block.Position.X + block.Size.X); x++) {
						for (int z = (int) block.Position.Z; z < (int) (block.Position.Z + block.Size.Z); z++) {
							if (cubes[x, (int) block.Position.Y + (int) block.Size.Y, z] == null) {
								cull = false;
								break;
							}
						}
					}


					if (cull) {
						block.CullFace(Face.Top);
					}
				}

				cull = true;
				// bottom face -y
				if (block.Position.Y - 1 >= 0) {
					for (int x = (int) block.Position.X; x < (int) (block.Position.X + block.Size.X); x++) {
						for (int z = (int) block.Position.Z; z < (int) (block.Position.Z + block.Size.Z); z++) {
							if (cubes[x, (int) block.Position.Y - 1, z] == null) {
								cull = false;
								break;
							}
						}
					}

					if (cull) {
						block.CullFace(Face.Bottom);
					}
				}
			}


			foreach (Block block in blocks) {
				points.AddRange(block.GetVerts());
			}

			/*bool[,,] plane = new bool[_chunksize, _chunksize, 6];
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
			}*/

			Vertex3D.BufferVertices(VBO, points.ToArray(), BufferUsageHint.StaticDraw);
		}

		private Block CreateBlock (int x, int y, int z, bool[,,] accounted) {
			int width = 1, height = 1, depth = 1;

			for (int i = x + 1; i < _chunksize; i++) {
				if (!accounted[i, y, z] && cubes[i, y, z] != null) {
					accounted[i, y, z] = true;
					width++;
				} else {
					break;
				}
			}

			for (int i = y + 1; i < _chunksize; i++) {
				bool row = true;
				for (int ii = 0; ii < width; ii++) {
					if (accounted[x + ii, i, z] || cubes[x + ii, i, z] == null) {
						row = false;
					}
				}

				if (row) {
					for (int ii = 0; ii < width; ii++) {
						accounted[x + ii, i, z] = true;
					}

					height++;
				}
			}

			for (int i = z + 1; i < _chunksize; i++) {
				bool expand = true;

				for (int ii = 0; ii < height; ii++) {
					bool row = true;

					for (int iii = 0; iii < width; iii++) {
						if (accounted[x + iii, ii, i] || cubes[x + iii, ii, i] == null) {
							row = false;
						}
					}

					if (!row) {
						expand = false;
						break;
					}
				}

				if (expand) {
					for (int ii = 0; ii < height; ii++) {
						for (int iii = 0; iii < width; iii++) {
							accounted[x + iii, y + ii, i] = true;
						}
					}

					depth++;
				} else {
					break;
				}
			}

			return new Block(new Vector3(x, y, z), new Vector3(width, height, depth));
		}

			public void Render () {
			GL.BindVertexArray(VAO);
			GL.DrawArrays(PrimitiveType.Triangles, 0, points.Count);
			GL.BindVertexArray(0);
		}

	}
}
