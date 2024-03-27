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
			new Vertex3D(1f, 1f, 1f),

			new Vertex3D(0f, 1f, 1f),
			new Vertex3D(1f, 0f, 1f),
			new Vertex3D(1f, 1f, 0f),

			new Vertex3D(0f, 0f, 1f),
			new Vertex3D(1f, 0f, 0f),
			new Vertex3D(0f, 1f, 0f),

			new Vertex3D(0f, 0f, 0f)
		};

		public static Vertex3D[] GetFace (Face face, Vector3 Position, Vector4 Colour, Vector3? Scale = null) {
			Vector3 scalar = new Vector3(1);
		
			if (Scale != null) {
				scalar = (Vector3) Scale;
			}

			switch (face) {
				case Face.Top:
					return new Vertex3D[] {
						// top face
						new Vertex3D(_vertices[0].Position * scalar, Position, Colour) { Normal = new Vector3(0, 1, 0) },
						new Vertex3D(_vertices[6].Position * scalar, Position, Colour) { Normal = new Vector3(0, 1, 0) },
						new Vertex3D(_vertices[1].Position * scalar, Position, Colour) { Normal = new Vector3(0, 1, 0) },

						new Vertex3D(_vertices[0].Position * scalar, Position, Colour) { Normal = new Vector3(0, 1, 0) },
						new Vertex3D(_vertices[3].Position * scalar, Position, Colour) { Normal = new Vector3(0, 1, 0) },
						new Vertex3D(_vertices[6].Position * scalar, Position, Colour) { Normal = new Vector3(0, 1, 0) }
					};

				case Face.Left:
					return new Vertex3D[] {
						// left face
						new Vertex3D(_vertices[1].Position * scalar, Position, Colour) { Normal = new Vector3(-1, 0, 0) },
						new Vertex3D(_vertices[6].Position * scalar, Position, Colour) { Normal = new Vector3(-1, 0, 0) },
						new Vertex3D(_vertices[7].Position * scalar, Position, Colour) { Normal = new Vector3(-1, 0, 0) },

						new Vertex3D(_vertices[1].Position * scalar, Position, Colour) { Normal = new Vector3(-1, 0, 0) },
						new Vertex3D(_vertices[7].Position * scalar, Position, Colour) { Normal = new Vector3(-1, 0, 0) },
						new Vertex3D(_vertices[4].Position * scalar, Position, Colour) { Normal = new Vector3(-1, 0, 0) },
					};

				case Face.Right:
					return new Vertex3D[] {
						// right face
						new Vertex3D(_vertices[2].Position * scalar, Position, Colour) { Normal = new Vector3(1, 0, 0) },
						new Vertex3D(_vertices[5].Position * scalar, Position, Colour) { Normal = new Vector3(1, 0, 0) },
						new Vertex3D(_vertices[3].Position * scalar, Position, Colour) { Normal = new Vector3(1, 0, 0) },

						new Vertex3D(_vertices[3].Position * scalar, Position, Colour) { Normal = new Vector3(1, 0, 0) },
						new Vertex3D(_vertices[0].Position * scalar, Position, Colour) { Normal = new Vector3(1, 0, 0) },
						new Vertex3D(_vertices[2].Position * scalar, Position, Colour) { Normal = new Vector3(1, 0, 0) },
					};

				case Face.Front:
					return new Vertex3D[] {
						// front face
						new Vertex3D(_vertices[2].Position * scalar, Position, Colour) { Normal = new Vector3(0, 0, 1) },
						new Vertex3D(_vertices[0].Position * scalar, Position, Colour) { Normal = new Vector3(0, 0, 1) },
						new Vertex3D(_vertices[1].Position * scalar, Position, Colour) { Normal = new Vector3(0, 0, 1) },

						new Vertex3D(_vertices[1].Position * scalar, Position, Colour) { Normal = new Vector3(0, 0, 1) },
						new Vertex3D(_vertices[4].Position * scalar, Position, Colour) { Normal = new Vector3(0, 0, 1) },
						new Vertex3D(_vertices[2].Position * scalar, Position, Colour) { Normal = new Vector3(0, 0, 1) },
					};

				case Face.Back:
					return new Vertex3D[] {
						// back face
						new Vertex3D(_vertices[6].Position * scalar, Position, Colour) { Normal = new Vector3(0, 0, -1) },
						new Vertex3D(_vertices[3].Position * scalar, Position, Colour) { Normal = new Vector3(0, 0, -1) },
						new Vertex3D(_vertices[5].Position * scalar, Position, Colour) { Normal = new Vector3(0, 0, -1) },

						new Vertex3D(_vertices[5].Position * scalar, Position, Colour) { Normal = new Vector3(0, 0, -1) },
						new Vertex3D(_vertices[7].Position * scalar, Position, Colour) { Normal = new Vector3(0, 0, -1) },
						new Vertex3D(_vertices[6].Position * scalar, Position, Colour) { Normal = new Vector3(0, 0, -1) },
					};

				case Face.Bottom:
					return new Vertex3D[] {
						// bottom face
						new Vertex3D(_vertices[7].Position * scalar, Position, Colour) { Normal = new Vector3(0, -1, 0) },
						new Vertex3D(_vertices[5].Position * scalar, Position, Colour) { Normal = new Vector3(0, -1, 0) },
						new Vertex3D(_vertices[2].Position * scalar, Position, Colour) { Normal = new Vector3(0, -1, 0) },

						new Vertex3D(_vertices[7].Position * scalar, Position, Colour) { Normal = new Vector3(0, -1, 0) },
						new Vertex3D(_vertices[2].Position * scalar, Position, Colour) { Normal = new Vector3(0, -1, 0) },
						new Vertex3D(_vertices[4].Position * scalar, Position, Colour) { Normal = new Vector3(0, -1, 0) },
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

		public static Vertex3D[] GetVerts (Vector3 Position, Vector4 Colour, Vector3? Scale = null) {
			Vector3 scalar = new Vector3(1);

			if (Scale != null) {
				scalar = (Vector3) Scale;
			}

			Vertex3D[] verts = {
				// top face
				new Vertex3D(_vertices[0].Position * scalar, Position, Colour) { Normal = new Vector3(0, 1, 0) },
				new Vertex3D(_vertices[6].Position * scalar, Position, Colour) { Normal = new Vector3(0, 1, 0) },
				new Vertex3D(_vertices[1].Position * scalar, Position, Colour) { Normal = new Vector3(0, 1, 0) },

				new Vertex3D(_vertices[0].Position * scalar, Position, Colour) { Normal = new Vector3(0, 1, 0) },
				new Vertex3D(_vertices[3].Position * scalar, Position, Colour) { Normal = new Vector3(0, 1, 0) },
				new Vertex3D(_vertices[6].Position * scalar, Position, Colour) { Normal = new Vector3(0, 1, 0) },

				// left face
				new Vertex3D(_vertices[1].Position * scalar, Position, Colour) { Normal = new Vector3(-1, 0, 0) },
				new Vertex3D(_vertices[6].Position * scalar, Position, Colour) { Normal = new Vector3(-1, 0, 0) },
				new Vertex3D(_vertices[7].Position * scalar, Position, Colour) { Normal = new Vector3(-1, 0, 0) },

				new Vertex3D(_vertices[1].Position * scalar, Position, Colour) { Normal = new Vector3(-1, 0, 0) },
				new Vertex3D(_vertices[7].Position * scalar, Position, Colour) { Normal = new Vector3(-1, 0, 0) },
				new Vertex3D(_vertices[4].Position * scalar, Position, Colour) { Normal = new Vector3(-1, 0, 0) },

				// right face
				new Vertex3D(_vertices[2].Position * scalar, Position, Colour) { Normal = new Vector3(1, 0, 0) },
				new Vertex3D(_vertices[5].Position * scalar, Position, Colour) { Normal = new Vector3(1, 0, 0) },
				new Vertex3D(_vertices[3].Position * scalar, Position, Colour) { Normal = new Vector3(1, 0, 0) },

				new Vertex3D(_vertices[3].Position * scalar, Position, Colour) { Normal = new Vector3(1, 0, 0) },
				new Vertex3D(_vertices[0].Position * scalar, Position, Colour) { Normal = new Vector3(1, 0, 0) },
				new Vertex3D(_vertices[2].Position * scalar, Position, Colour) { Normal = new Vector3(1, 0, 0) },

				// front face
				new Vertex3D(_vertices[2].Position * scalar, Position, Colour) { Normal = new Vector3(0, 0, 1) },
				new Vertex3D(_vertices[0].Position * scalar, Position, Colour) { Normal = new Vector3(0, 0, 1) },
				new Vertex3D(_vertices[1].Position * scalar, Position, Colour) { Normal = new Vector3(0, 0, 1) },

				new Vertex3D(_vertices[1].Position * scalar, Position, Colour) { Normal = new Vector3(0, 0, 1) },
				new Vertex3D(_vertices[4].Position * scalar, Position, Colour) { Normal = new Vector3(0, 0, 1) },
				new Vertex3D(_vertices[2].Position * scalar, Position, Colour) { Normal = new Vector3(0, 0, 1) },

				// back face
				new Vertex3D(_vertices[6].Position * scalar, Position, Colour) { Normal = new Vector3(0, 0, -1) },
				new Vertex3D(_vertices[3].Position * scalar, Position, Colour) { Normal = new Vector3(0, 0, -1) },
				new Vertex3D(_vertices[5].Position * scalar, Position, Colour) { Normal = new Vector3(0, 0, -1) },

				new Vertex3D(_vertices[5].Position * scalar, Position, Colour) { Normal = new Vector3(0, 0, -1) },
				new Vertex3D(_vertices[7].Position * scalar, Position, Colour) { Normal = new Vector3(0, 0, -1) },
				new Vertex3D(_vertices[6].Position * scalar, Position, Colour) { Normal = new Vector3(0, 0, -1) },

				// bottom face
				new Vertex3D(_vertices[7].Position * scalar, Position, Colour) { Normal = new Vector3(0, -1, 0) },
				new Vertex3D(_vertices[5].Position * scalar, Position, Colour) { Normal = new Vector3(0, -1, 0) },
				new Vertex3D(_vertices[2].Position * scalar, Position, Colour) { Normal = new Vector3(0, -1, 0) },

				new Vertex3D(_vertices[7].Position * scalar, Position, Colour) { Normal = new Vector3(0, -1, 0) },
				new Vertex3D(_vertices[2].Position * scalar, Position, Colour) { Normal = new Vector3(0, -1, 0) },
				new Vertex3D(_vertices[4].Position * scalar, Position, Colour) { Normal = new Vector3(0, -1, 0) },
			};

			return verts;
		}
	}

	public enum Face {
		Front = 0,
		Back = 1,
		Left = 2,
		Right = 3,
		Top = 4,
		Bottom = 5
	}
}
