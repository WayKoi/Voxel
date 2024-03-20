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

		public static Vertex3D[] getFace (Face face, Vector3 Position, Vector4 Colour) {
			switch (face) {
				case Face.Top:
					return new Vertex3D[] {
						// top face
						new Vertex3D(_vertices[0], Position, Colour) { Normal = new Vector3(0, 1, 0) },
						new Vertex3D(_vertices[6], Position, Colour) { Normal = new Vector3(0, 1, 0) },
						new Vertex3D(_vertices[1], Position, Colour) { Normal = new Vector3(0, 1, 0) },

						new Vertex3D(_vertices[0], Position, Colour) { Normal = new Vector3(0, 1, 0) },
						new Vertex3D(_vertices[3], Position, Colour) { Normal = new Vector3(0, 1, 0) },
						new Vertex3D(_vertices[6], Position, Colour) { Normal = new Vector3(0, 1, 0) }
					};

				case Face.Left:
					return new Vertex3D[] {
						// left face
						new Vertex3D(_vertices[1], Position, Colour) { Normal = new Vector3(-1, 0, 0) },
						new Vertex3D(_vertices[6], Position, Colour) { Normal = new Vector3(-1, 0, 0) },
						new Vertex3D(_vertices[7], Position, Colour) { Normal = new Vector3(-1, 0, 0) },

						new Vertex3D(_vertices[1], Position, Colour) { Normal = new Vector3(-1, 0, 0) },
						new Vertex3D(_vertices[7], Position, Colour) { Normal = new Vector3(-1, 0, 0) },
						new Vertex3D(_vertices[4], Position, Colour) { Normal = new Vector3(-1, 0, 0) },
					};

				case Face.Right:
					return new Vertex3D[] {
						// right face
						new Vertex3D(_vertices[2], Position, Colour) { Normal = new Vector3(1, 0, 0) },
						new Vertex3D(_vertices[5], Position, Colour) { Normal = new Vector3(1, 0, 0) },
						new Vertex3D(_vertices[3], Position, Colour) { Normal = new Vector3(1, 0, 0) },

						new Vertex3D(_vertices[3], Position, Colour) { Normal = new Vector3(1, 0, 0) },
						new Vertex3D(_vertices[0], Position, Colour) { Normal = new Vector3(1, 0, 0) },
						new Vertex3D(_vertices[2], Position, Colour) { Normal = new Vector3(1, 0, 0) },
					};

				case Face.Front:
					return new Vertex3D[] {
						// front face
						new Vertex3D(_vertices[2], Position, Colour) { Normal = new Vector3(0, 0, 1) },
						new Vertex3D(_vertices[0], Position, Colour) { Normal = new Vector3(0, 0, 1) },
						new Vertex3D(_vertices[1], Position, Colour) { Normal = new Vector3(0, 0, 1) },

						new Vertex3D(_vertices[1], Position, Colour) { Normal = new Vector3(0, 0, 1) },
						new Vertex3D(_vertices[4], Position, Colour) { Normal = new Vector3(0, 0, 1) },
						new Vertex3D(_vertices[2], Position, Colour) { Normal = new Vector3(0, 0, 1) },
					};

				case Face.Back:
					return new Vertex3D[] {
						// back face
						new Vertex3D(_vertices[6], Position, Colour) { Normal = new Vector3(0, 0, -1) },
						new Vertex3D(_vertices[3], Position, Colour) { Normal = new Vector3(0, 0, -1) },
						new Vertex3D(_vertices[5], Position, Colour) { Normal = new Vector3(0, 0, -1) },

						new Vertex3D(_vertices[5], Position, Colour) { Normal = new Vector3(0, 0, -1) },
						new Vertex3D(_vertices[7], Position, Colour) { Normal = new Vector3(0, 0, -1) },
						new Vertex3D(_vertices[6], Position, Colour) { Normal = new Vector3(0, 0, -1) },
					};

				case Face.Bottom:
					return new Vertex3D[] {
						// bottom face
						new Vertex3D(_vertices[7], Position, Colour) { Normal = new Vector3(0, -1, 0) },
						new Vertex3D(_vertices[5], Position, Colour) { Normal = new Vector3(0, -1, 0) },
						new Vertex3D(_vertices[2], Position, Colour) { Normal = new Vector3(0, -1, 0) },

						new Vertex3D(_vertices[7], Position, Colour) { Normal = new Vector3(0, -1, 0) },
						new Vertex3D(_vertices[2], Position, Colour) { Normal = new Vector3(0, -1, 0) },
						new Vertex3D(_vertices[4], Position, Colour) { Normal = new Vector3(0, -1, 0) },
					};
			}

			return new Vertex3D[0];
		}

		public Cube() {
			Colour = new Vector4(255, 255, 255, 255);
		}

		public Cube (Vector4 colour) {
			Colour = colour;
		}

		public Vertex3D[] GetVerts (Vector3 Position) {
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

	public enum Face {
		Front,
		Back,
		Left,
		Right,
		Top,
		Bottom
	}
}
