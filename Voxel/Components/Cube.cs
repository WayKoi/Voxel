using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voxel.Structs;

namespace Voxel.Components {
	internal struct Cube {
		public Vector4 Colour;
		public Vector3 Position;

		private static Vertex3D[] _vertices = {
			new Vertex3D(0.5f, 0.5f, 0.5f),

			new Vertex3D(-0.5f, 0.5f, 0.5f),
			new Vertex3D(0.5f, -0.5f, 0.5f),
			new Vertex3D(0.5f, 0.5f, -0.5f),

			new Vertex3D(-0.5f, -0.5f, 0.5f),
			new Vertex3D(0.5f, -0.5f, -0.5f),
			new Vertex3D(-0.5f, 0.5f, -0.5f),

			new Vertex3D(-0.5f, -0.5f, -0.5f)
		};

		public Cube() {
			Colour = new Vector4(255, 255, 255, 255);
			Position = new Vector3();
		}

		public Cube (Vector4 colour, Vector3 position) {
			Colour = colour;
			Position = position;
		}

		public Vertex3D[] GetVerts () {
			Vertex3D[] verts = {
				// top face
				new Vertex3D(_vertices[0], Position, Colour),
				new Vertex3D(_vertices[6], Position, Colour),
				new Vertex3D(_vertices[1], Position, Colour),

				new Vertex3D(_vertices[0], Position, Colour),
				new Vertex3D(_vertices[3], Position, Colour),
				new Vertex3D(_vertices[6], Position, Colour),

				// left face
				new Vertex3D(_vertices[1], Position, Colour),
				new Vertex3D(_vertices[6], Position, Colour),
				new Vertex3D(_vertices[7], Position, Colour),

				new Vertex3D(_vertices[1], Position, Colour),
				new Vertex3D(_vertices[7], Position, Colour),
				new Vertex3D(_vertices[4], Position, Colour),

				// right face
				new Vertex3D(_vertices[2], Position, Colour),
				new Vertex3D(_vertices[5], Position, Colour),
				new Vertex3D(_vertices[3], Position, Colour),

				new Vertex3D(_vertices[3], Position, Colour),
				new Vertex3D(_vertices[0], Position, Colour),
				new Vertex3D(_vertices[2], Position, Colour),

				// front face
				new Vertex3D(_vertices[2], Position, Colour),
				new Vertex3D(_vertices[0], Position, Colour),
				new Vertex3D(_vertices[1], Position, Colour),

				new Vertex3D(_vertices[1], Position, Colour),
				new Vertex3D(_vertices[4], Position, Colour),
				new Vertex3D(_vertices[2], Position, Colour),

				// back face
				new Vertex3D(_vertices[6], Position, Colour),
				new Vertex3D(_vertices[3], Position, Colour),
				new Vertex3D(_vertices[5], Position, Colour),

				new Vertex3D(_vertices[5], Position, Colour),
				new Vertex3D(_vertices[7], Position, Colour),
				new Vertex3D(_vertices[6], Position, Colour),

				// bottom face
				new Vertex3D(_vertices[7], Position, Colour),
				new Vertex3D(_vertices[5], Position, Colour),
				new Vertex3D(_vertices[2], Position, Colour),

				new Vertex3D(_vertices[7], Position, Colour),
				new Vertex3D(_vertices[2], Position, Colour),
				new Vertex3D(_vertices[4], Position, Colour),
			};

			return verts;
		}
	}
}
