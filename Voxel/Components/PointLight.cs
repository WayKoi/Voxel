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
	public class PointLight {
		public Light Light = new Light(0, 0, 0);
		private Vector3 _position = new Vector3(0);

		public Vector3 Position {
			get { return _position; }
			set { 
				_position = value;
				Update();
			}
		}

		public float Distance = 10;

		private int _vbo, _vao;
		private bool _loaded = false;

		public PointLight () { }

		public PointLight(Vector3 colour, Vector3 pos, float distance) {
			Light = new Light(new Vector3(0), colour * 0.75f, colour);
			Position = pos;
			Distance = distance;
		}

		public PointLight(Light light, Vector3 pos, float distance) {
			Light = light;
			Position = pos;
			Distance = distance;
		}

		public PointLight(string name, Vector3 pos, float distance) {
			Light = Light.GetLight(name);
			Position = pos;
			Distance = distance;
		}

		public void Load () {
			if (_loaded) { return; }
			_vbo = Vertex3D.GenVBO(Cube.GetVerts(Position, new Vector4(Light.Specular, 1)), BufferUsageHint.StaticDraw);
			_vao = Vertex3D.GenVAO(_vbo);
			_loaded = true;
		}

		private void Update () {
			Vertex3D.BufferVertices(_vbo, Cube.GetVerts(Position, new Vector4(Light.Specular, 1)), BufferUsageHint.StaticDraw);
		}

		public void AddToShader (int index, Shader shader) {
			if (!_loaded) { return; } 

			shader.SetVector3("lights[" + index + "].position", Position);
			shader.SetVector3("lights[" + index + "].diffuse", Light.Diffuse);
			shader.SetVector3("lights[" + index + "].specular", Light.Specular);
			shader.SetFloat("lights[" + index + "].distance", Distance);
		}

		public void Render (Shader shader) {
			if (!_loaded) { return; }

			shader.SetVector3("lightColour", Light.Specular);

			GL.BindVertexArray(_vao);
			GL.DrawArrays(PrimitiveType.Triangles, 0, 6 * 6);
			GL.BindVertexArray(0);
		}

	}
}
