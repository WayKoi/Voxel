using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Platform.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voxel.Structs;

namespace Voxel.Components {
	internal class Chunk {
		public static readonly short Size = 32;

		private Vector3 _position = new Vector3();

		private int[,,] _cubetypes;
		private int[,] _filled; // we use this one bitwise

		private bool _loaded = false, _disposed = false;

		private int _vao, _vbo, _pointCount = 0;

		public Chunk(Vector3 position) {
			_position = position;

			_cubetypes = new int[Size, Size, Size];
			_filled = new int[Size, Size];
		}

		public void AddCubes (int id, int x, int y, int z) {
			if (x < 0 || x >= Size || y < 0 || y >= Size || z < 0 || z >= Size) { 
				Error.Report("Misplaced Cube at pos " + x + "  " + y + "  " + z);
				return; 
			}

			_cubetypes[x, y, z] = id;
			if (!CheckFilled(x, y, z)) {
				_filled[y, z] += 1 << x; // add a single bit to the proper position
			}
		}

		// returns -1 if one is not found
		public int GetTopCube (int x, int z) {
			for (int y = Size - 1; y >= 0; y--) {
				if (CheckSpot(_filled[y, z], x)) {
					return y;
				}
			}

			return -1;
		}

		public float GetDistance (Vector3 from) {
			Vector3 center = _position + new Vector3((float) (Size / 2.0));
			return Vector3.Distance(center, from);
		}

		public float GetAngle(Vector3 point, Vector3 from) {
			Vector3 center = _position + new Vector3((float) (Size / 2.0)) - point;
			return Vector3.CalculateAngle(center, from);
		}

		public int[] GetEdge (Face face) {
			List<int> lines = new List<int>();
			
			switch (face) {
				case Face.Left:
					for (int y = 0; y < Size; y++) {
						int line = 0;
						
						for (int z = 0; z < Size; z++) {
							int spot = GetSpot(_filled[y, z], 0);
							line += spot << z;
						}

						lines.Add(line);
					}
					break;
				case Face.Right:
					for (int y = 0; y < Size; y++) {
						int line = 0;

						for (int z = 0; z < Size; z++) {
							int spot = GetSpot(_filled[y, z], 31);
							int shift = GetSpot((spot >> 31), 0);
							line += shift << z;
						}

						lines.Add(line);
					}
					break;
				case Face.Top:
					for (int z = 0; z < Size; z++) {
						lines.Add(_filled[31, z]);
					}
					break;
				case Face.Bottom:
					for (int z = 0; z < Size; z++) {
						lines.Add(_filled[0, z]);
					}
					break;
				case Face.Front:
					for (int y = 0; y < Size; y++) {
						lines.Add(_filled[y, 31]);
					}
					break;
				case Face.Back:
					for (int y = 0; y < Size; y++) {
						lines.Add(_filled[y, 0]);
					}
					break;
			}

			return lines.ToArray();
		}

		public void Load (int[][]? edges = null) {
			if (_loaded || _disposed) { return; }

			int[][] checks = new int[6][];

			if (edges != null) {
				checks = edges;
			}

			for (int i = 0; i < 6; i++) {
				if (checks[i] == null) {
					checks[i] = new int[Size];
				}
			}

			List<Vertex3D> load = new List<Vertex3D>();

			for (int y = 0; y < Size; y++) {
				for (int z = 0; z < Size; z++) {
					// cull faces
					int left = _filled[y, z] << 1;

					// check for left chunk being full
					if (CheckSpot(checks[(int) Face.Left][y], z)) {
						left += 1; // this means that the chunk next to this one covers this face
					}

					left = left ^ (_filled[y, z]);

					int right = _filled[y, z] >> 1;

					// remove the high bit if it is set only if the chunk next to it is empty
					bool check = CheckSpot(checks[(int) Face.Right][y], z);

					if (CheckSpot(right, 31)) {
						if (!check) {
							right -= (1 << 31);
						}
					} else if (check) {
						right += (1 << 31);
					}

					right = _filled[y, z] ^ right;

					int top, bottom;

					if (y < Size - 1) {
						top = _filled[y, z] ^ _filled[y + 1, z];
					} else {
						top = _filled[y, z] ^ checks[(int) Face.Top][z];
					}

					if (y > 0) {
						bottom = _filled[y, z] ^ _filled[y - 1, z];
					} else {
						bottom = _filled[y, z] ^ checks[(int) Face.Bottom][z];
					}

					int front, back;

					if (z < Size - 1) {
						front = _filled[y, z] ^ _filled[y, z + 1];
					} else {
						front = _filled[y, z] ^ checks[(int) Face.Front][y];
					}

					if (z > 0) {
						back = _filled[y, z] ^ _filled[y, z - 1];
					} else {
						back = _filled[y, z] ^ checks[(int) Face.Back][y];
					}

					float py = _position.Y + y;
					float pz = _position.Z + z;

					for (int x = 0; x < Size; x++) {
						float px = _position.X + x;

						if (CheckSpot(_filled[y, z], x)) {
							Vector4 colour = Cube.GetCubeColour(_cubetypes[x, y, z], px, py, pz);

							if (CheckSpot(left, x)) {
								load.AddRange(Cube.GetFace(Face.Left, px, py, pz, colour));
							}

							if (CheckSpot(right, x)) {
								load.AddRange(Cube.GetFace(Face.Right, px, py, pz, colour));
							}

							if (CheckSpot(top, x)) {
								load.AddRange(Cube.GetFace(Face.Top, px, py, pz, colour));
							}

							if (CheckSpot(bottom, x)) {
								load.AddRange(Cube.GetFace(Face.Bottom, px, py, pz, colour));
							}

							if (CheckSpot(front, x)) {
								load.AddRange(Cube.GetFace(Face.Front, px, py, pz, colour));
							}

							if (CheckSpot(back, x)) {
								load.AddRange(Cube.GetFace(Face.Back, px, py, pz, colour));
							}
						}
					}
				}
			}

			_vbo = Vertex3D.GenVBO(load.ToArray(), BufferUsageHint.StaticDraw);
			_vao = Vertex3D.GenVAO(_vbo);
			_pointCount = load.Count;

			_loaded = true;
		}

		public void Unload () {
			if (!_loaded) { return; }

			GL.DeleteVertexArray(_vao);
			GL.DeleteBuffer(_vbo);
			_pointCount = 0;

			_loaded = false;
		}

		public void Dispose () {
			if (_disposed) { return; }

			Unload();

			_disposed = true;
		}

		public void Render () {
			if (!_loaded || _disposed) { return; }

			GL.BindVertexArray(_vao);
			GL.DrawArrays(PrimitiveType.Triangles, 0, _pointCount);
			GL.BindVertexArray(0);
		}


		// Private
		private bool CheckFilled (int x, int y, int z) {
			return (_filled[y, z] & 1 << x) != 0;
		}

		private bool CheckSpot (int row, int pos) {
			return (row & 1 << pos) != 0;
		}

		private int GetSpot (int row, int pos) {
			return row & (1 << pos);
		}
	}
}
