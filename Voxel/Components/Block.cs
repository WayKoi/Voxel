using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voxel.Structs;

namespace Voxel.Components {
	internal class Block {
		public Vector3 Position;
		public Vector3 Size;

		// private Texture _texture;

		// private int _vbo, _vao;

		// left right top bottom front back
		private bool[] cull = new bool[6];

		public Block(Vector3 pos, Vector3 size/*, List<Cube> cubes, Face[] covered*/) {
			Position = pos;
			Size = size;
			// _texture = new Texture();
		}

		/*public Vertex3D[] GetVerts () {
			List<Vertex3D> points = new List<Vertex3D>();

			for (int i = 0; i < cull.Length; i++) {
				if (!cull[i]) {
					points.AddRange(Cube.GetFace((Face) i, Position, Vector4.One, Size));
				}
			}

			return points.ToArray();
		}	*/

		public void CullFace (Face face) {
			cull[(int) face] = true;
		}

		/*private Texture GenTexture(List<Cube> cubes) {
			Texture tex = new Texture();

			int width = Size.X

			List<byte> data = new List<byte>();


		}*/

	}
}
