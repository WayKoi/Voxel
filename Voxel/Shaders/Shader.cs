using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voxel.Shaders {
	public class Shader {
		private bool _loaded = false;
		private int _handle;
		private int _vertexShader, _fragmentShader;

		private string _vertexPath, _fragmentPath;

		public Shader(string vertexPath, string fragmentPath) {
			_vertexPath = vertexPath;
			_fragmentPath = fragmentPath;
		}

		public void Load() {
			if (_loaded) { return; }

			string _vertexShaderSource = File.ReadAllText(_vertexPath);
			string _fragmentShaderSource = File.ReadAllText(_fragmentPath);

			_vertexShader = GL.CreateShader(ShaderType.VertexShader);
			GL.ShaderSource(_vertexShader, _vertexShaderSource);

			_fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
			GL.ShaderSource(_fragmentShader, _fragmentShaderSource);

			GL.CompileShader(_vertexShader);

			int success;

			GL.GetShader(_vertexShader, ShaderParameter.CompileStatus, out success);
			if (success == 0) {
				string infoLog = GL.GetShaderInfoLog(_vertexShader);
				Console.WriteLine(infoLog);
			}

			GL.CompileShader(_fragmentShader);

			GL.GetShader(_fragmentShader, ShaderParameter.CompileStatus, out success);
			if (success == 0) {
				string infoLog = GL.GetShaderInfoLog(_fragmentShader);
				Console.WriteLine(infoLog);
			}

			_handle = GL.CreateProgram();

			GL.AttachShader(_handle, _vertexShader);
			GL.AttachShader(_handle, _fragmentShader);

			GL.LinkProgram(_handle);

			GL.GetProgram(_handle, GetProgramParameterName.LinkStatus, out success);
			if (success == 0) {
				string infoLog = GL.GetProgramInfoLog(_handle);
				Console.WriteLine(infoLog);
			}

			GL.DetachShader(_handle, _vertexShader);
			GL.DetachShader(_handle, _fragmentShader);
			GL.DeleteShader(_fragmentShader);
			GL.DeleteShader(_vertexShader);

			_loaded = true;
		}

		public void Use() {
			if (!_loaded) { return; }
			GL.UseProgram(_handle);
		}

		public void SetMatrix4(string handle, Matrix4 matrix) {
			GL.UseProgram(_handle);
			int location = GL.GetUniformLocation(_handle, handle);
			GL.UniformMatrix4(location, true, ref matrix);
		}

		public void SetVector3(string handle, Vector3 vect) {
			GL.UseProgram(_handle);
			int location = GL.GetUniformLocation(_handle, handle);
			GL.Uniform3(location, vect);
		}

		public void SetFloat(string handle, float vect) {
			GL.UseProgram(_handle);
			int location = GL.GetUniformLocation(_handle, handle);
			GL.Uniform1(location, vect);
		}

		public void SetInt(string handle, int inte) {
			GL.UseProgram(_handle);
			int location = GL.GetUniformLocation(_handle, handle);
			GL.Uniform1(location, inte);
		}

		private bool _disposed = false;

		protected virtual void Dispose(bool disposing) {
			if (!_disposed) {
				GL.DeleteProgram(_handle);
				_loaded = false;
				_disposed = true;
			}
		}

		~Shader() {
			if (!_disposed) {
				Console.WriteLine("GPU Resource leak! Did you forget to call Dispose()?");
			}
		}

		public int GetAttribLocation(string attribName) {
			return GL.GetAttribLocation(_handle, attribName);
		}


		public void Dispose() {
			Dispose(true);
			GC.SuppressFinalize(this);
		}
	}
}
