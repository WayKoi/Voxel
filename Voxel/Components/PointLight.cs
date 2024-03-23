using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voxel.Shaders;
using Voxel.Structs;

namespace Voxel.Components {
	internal class PointLight {
		public Vector3 Diffuse = new Vector3(1);
		public Vector3 Specular = new Vector3(1);

		private Vector3 _position = new Vector3(0);

		public Vector3 Position {
			get { return _position; }
			set { 
				_position = value;
				Update();
			}
		}

		private int _vbo, _vao;
		
		public float Distance = 10;

		public PointLight () { }

		public PointLight(Vector3 colour, Vector3 pos, float distance) {
			Diffuse = colour * 0.75f;
			Specular = colour;

			Position = pos;

			Distance = distance;
		}

		public void Load () {
			_vbo = Vertex3D.GenVBO(Cube.GetVerts(Position, new Vector4(Specular, 1)), BufferUsageHint.StaticDraw);
			_vao = Vertex3D.GenVAO(_vbo);
		}

		private void Update () {
			Vertex3D.BufferVertices(_vbo, Cube.GetVerts(Position, new Vector4(Specular, 1)), BufferUsageHint.StaticDraw);
		}

		public void AddToShader (int index, Shader shader) {
			shader.SetVector3("lights[" + index + "].position", Position);
			shader.SetVector3("lights[" + index + "].diffuse", Diffuse);
			shader.SetVector3("lights[" + index + "].specular", Specular);
			shader.SetFloat("lights[" + index + "].distance", Distance);
		}

		public void Render (Shader shader) {
			shader.SetVector3("lightColour", Specular);

			GL.BindVertexArray(_vao);
			GL.DrawArrays(PrimitiveType.Triangles, 0, 6 * 6);
			GL.BindVertexArray(0);
		}

	}
}
