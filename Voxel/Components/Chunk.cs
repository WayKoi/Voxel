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

		private bool _loaded = false;

		private int _vao, _vbo, _pointCount = 0;

		public Chunk(Vector3 position) {
			_position = position;

			_cubetypes = new int[Size, Size, Size];
			_filled = new int[Size, Size];
		}

		public void AddCubes (int id, int x, int y, int z) {
			if (x < 0 || x >= Size) { return; }
			if (y < 0 || y >= Size) { return; }
			if (z < 0 || z >= Size) { return; }

			_cubetypes[x, y, z] = id;
			if (!CheckFilled(x, y, z)) {
				_filled[y, z] += 1 << x; // add a single bit to the proper position
			}
		}

		public void Load () {
			if (_loaded) { return; }

			List<Vertex3D> load = new List<Vertex3D>();

			for (int y = 0; y < Size; y++) {
				for (int z = 0; z < Size; z++) {
					// cull faces
					int left = _filled[y, z] ^ (_filled[y, z] << 1);
					int right = _filled[y, z] >> 1;

					if (CheckSpot(right, 31)) { // remove the high bit if it is set
						right -= (1 << 31);
					}

					right = _filled[y, z] ^ right;

					int top = _filled[y, z];
					int bottom = _filled[y, z];

					if (y < Size - 1) {
						top = _filled[y, z] ^ _filled[y + 1, z];
					}

					if (y > 0) {
						bottom = _filled[y, z] ^ _filled[y - 1, z];
					}

					int front = _filled[y, z];
					int back = _filled[y, z];

					if (z < Size - 1) {
						front = _filled[y, z] ^ _filled[y, z + 1];
					}

					if (z > 0) {
						back = _filled[y, z] ^ _filled[y, z - 1];
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

		public void Render () {
			if (!_loaded) { return; }

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
