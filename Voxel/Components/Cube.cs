using OpenTK.Graphics.ES11;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voxel.Structs;

namespace Voxel.Components {
	public static class Cube {
		private static Dictionary<int, CubeType> _types = new Dictionary<int, CubeType>();
		private static int _counter = 1;

		private static CubeType _default = new CubeType(1, 1);

		public static CubeType GetCube (int id) {
			if (_types.ContainsKey(id)) {
				return _types[id];
			}

			return _default;
		}

		public static Vector4 GetCubeColour(int id, float x, float y, float z) {
			if (_types.ContainsKey(id)) {
				return _types[id].GetColour(x, y, z);
			}

			return _default.GetColour(x, y, z);
		}

		public static int AddType (CubeType type) {
			_types.Add(_counter, type);
			return _counter++;
		}

		public static Vertex3D[] GetVerts (Vector3 positon, Vector4 colour) {
			List<Vertex3D> points = new List<Vertex3D>();

			points.AddRange(GetFace(Face.Left, positon.X, positon.Y, positon.Z, colour));
			points.AddRange(GetFace(Face.Right, positon.X, positon.Y, positon.Z, colour));
			points.AddRange(GetFace(Face.Top, positon.X, positon.Y, positon.Z, colour));
			points.AddRange(GetFace(Face.Bottom, positon.X, positon.Y, positon.Z, colour));
			points.AddRange(GetFace(Face.Front, positon.X, positon.Y, positon.Z, colour));
			points.AddRange(GetFace(Face.Back, positon.X, positon.Y, positon.Z, colour));

			return points.ToArray();
		}

		public static Vertex3D[] GetFace (Face face, float x, float y, float z, Vector4 colour, Vector3? sizevect = null) {
			Vector3 size = Vector3.One;
		
			if (sizevect != null) {
				size = (Vector3) sizevect;
			}

			switch (face) {
				case Face.Back:
					return new Vertex3D[] {
						new Vertex3D(x, y,				 z - size.Z,	0, 0, -1, colour),
						new Vertex3D(x, y + size.Y,		 z - size.Z,	0, 0, -1, colour),
						new Vertex3D(x + size.X, y + size.Y, z - size.Z,	0, 0, -1, colour),

						new Vertex3D(x + size.X, y + size.Y, z - size.Z,  0, 0, -1, colour),
						new Vertex3D(x + size.X, y,		 z - size.Z,	0, 0, -1, colour),
						new Vertex3D(x, y,               z - size.Z,  0, 0, -1, colour)
					};
				case Face.Front:
					return (new Vertex3D[] {
						new Vertex3D(x, y, z,				0, 0, 1, colour),
						new Vertex3D(x, y + size.Y, z,		0, 0, 1, colour),
						new Vertex3D(x + size.X, y + size.Y, z, 0, 0, 1, colour),

						new Vertex3D(x + size.X, y + size.Y, z, 0, 0, 1, colour),
						new Vertex3D(x + size.X, y, z,		0, 0, 1, colour),
						new Vertex3D(x, y, z,               0, 0, 1, colour)
					}).Reverse().ToArray();
				case Face.Left:
					return new Vertex3D[] {
						new Vertex3D(x, y, z,				-1, 0, 0, colour),
						new Vertex3D(x, y + size.Y, z,		-1, 0, 0, colour),
						new Vertex3D(x, y + size.Y, z - size.Z, -1, 0, 0, colour),

						new Vertex3D(x, y + size.Y, z - size.Z, -1, 0, 0, colour),
						new Vertex3D(x, y, z - size.Z,		-1, 0, 0, colour),
						new Vertex3D(x, y, z,               -1, 0, 0, colour),
					};
				case Face.Right:
					return new Vertex3D[] {
						new Vertex3D(x + size.X, y, z,				1, 0, 0, colour),
						new Vertex3D(x + size.X, y + size.Y, z,			1, 0, 0, colour),
						new Vertex3D(x + size.X, y + size.Y, z - size.Z,	1, 0, 0, colour),

						new Vertex3D(x + size.X, y + size.Y, z - size.Z,  1, 0, 0, colour),
						new Vertex3D(x + size.X, y, z - size.Z,			1, 0, 0, colour),
						new Vertex3D(x + size.X, y, z,                1, 0, 0, colour),
					}.Reverse().ToArray(); ;
				case Face.Bottom:
					return new Vertex3D[] {
						new Vertex3D(x, y, z,				0, -1, 0, colour),
						new Vertex3D(x + size.X, y, z,		0, -1, 0, colour),
						new Vertex3D(x + size.X, y, z - size.Z, 0, -1, 0, colour),

						new Vertex3D(x + size.X, y, z - size.Z, 0, -1, 0, colour),
						new Vertex3D(x, y, z - size.Z,		0, -1, 0, colour),
						new Vertex3D(x, y, z,               0, -1, 0, colour),
					}.Reverse().ToArray(); ;
				case Face.Top:
					return new Vertex3D[] {
						new Vertex3D(x, y + size.Y, z,				0, 1, 0, colour),
						new Vertex3D(x + size.X, y + size.Y, z,			0, 1, 0, colour),
						new Vertex3D(x + size.X, y + size.Y, z - size.Z,	0, 1, 0, colour),

						new Vertex3D(x + size.X, y + size.Y, z - size.Z,	0, 1, 0, colour),
						new Vertex3D(x, y + size.Y, z - size.Z,			0, 1, 0, colour),
						new Vertex3D(x, y + size.Y, z,				0, 1, 0, colour),
					};
			}

			return new Vertex3D[] {};
		}
	}

	public enum Face {
		Back = 0,
		Front = 1,
		Left = 2,
		Right = 3,
		Bottom = 4,
		Top = 5,
	}
}
