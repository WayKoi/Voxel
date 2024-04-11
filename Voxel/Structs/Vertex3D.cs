using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voxel.Structs {
	public struct Vertex3D {
		public Vector3 Position;
		public Vector3 Normal;
		public Vector4 Colour;

		public Vertex3D (float x, float y, float z, float nx, float ny, float nz, Vector4? colour = null) {
			Position.X = x;
			Position.Y = y;
			Position.Z = z;

			Normal.X = nx;
			Normal.Y = ny;
			Normal.Z = nz;

			Colour = Vector4.One;
			if (colour != null) {
				Colour = (Vector4) colour;
			}
		}

		public Vertex3D(float x, float y, float z, Vector4? colour = null) {
			Position.X = x;
			Position.Y = y;
			Position.Z = z;

			Normal.X = 0;
			Normal.Y = 1;
			Normal.Z = 0;

			Colour = Vector4.One;
			if (colour != null) {
				Colour = (Vector4) colour;
			}
		}

		public Vertex3D (Vertex3D start, Vector3 shift, Vector4? colour = null) {
			Position = start.Position;

			Position.X += shift.X;
			Position.Y += shift.Y;
			Position.Z += shift.Z;

			Normal = start.Normal;

			Colour = Vector4.One;
			if (colour != null) {
				Colour = (Vector4) colour;
			}
		}

		public Vertex3D(Vector3 start, Vector3 shift, Vector4? colour = null) {
			Position = start;

			Position.X += shift.X;
			Position.Y += shift.Y;
			Position.Z += shift.Z;

			Normal = new Vector3();

			Colour = Vector4.One;
			if (colour != null) {
				Colour = (Vector4) colour;
			}
		}

		public static int SizeInBytes {
			get { return Vector3.SizeInBytes + Vector3.SizeInBytes + Vector4.SizeInBytes; }
		}

		public static int GenVBO(Vertex3D[] Draw, BufferUsageHint hint = BufferUsageHint.DynamicDraw) {
			int VBO = GL.GenBuffer();

			BufferVertices(VBO, Draw, hint);

			return VBO;
		}

		public static void BufferVertices(int VBO, Vertex3D[] Draw, BufferUsageHint hint = BufferUsageHint.DynamicDraw) {
			GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
			GL.BufferData(BufferTarget.ArrayBuffer, SizeInBytes * Draw.Length, Draw, hint);
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
		}

		public static int GenVAO(int VBO) {
			int VAO = GL.GenVertexArray();

			GL.BindVertexArray(VAO);

			GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);

			// position
			GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, SizeInBytes, 0);
			GL.EnableVertexAttribArray(0);

			// normal
			GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, SizeInBytes, Vector3.SizeInBytes);
			GL.EnableVertexAttribArray(1);

			// colour
			GL.VertexAttribPointer(2, 4, VertexAttribPointerType.Float, false, SizeInBytes, Vector3.SizeInBytes * 2);
			GL.EnableVertexAttribArray(2);

			GL.BindVertexArray(0);
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

			return VAO;
		}

		public static Vector3 Normalize(Vector3 input) {
			float size = input.Length;

			return input / size;
		}

		public static Vector3 Cross(Vector3 a, Vector3 b) {
			return new Vector3(
				a.Y * b.Z - a.Z * b.Y,
				a.Z * b.X - a.X * b.Z,
				a.X * b.Y - a.Y * b.X
			);
		}

		public static float Dot(Vector3 a, Vector3 b) {
			return a.X * b.X + a.Y * b.Y + a.Z * b.Z;
		}
	}
}
