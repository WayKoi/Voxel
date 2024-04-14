using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.PortableExecutable;
using System.Text;
using Voxel.Shaders;
using Voxel.Structs;

namespace Voxel.Components {
	public class World {
		private Dictionary<(int, int, int), Chunk> _chunks = new Dictionary<(int, int, int), Chunk>();
		private bool _initialized = false, _loaded = false, _disposed = false;

		private Matrix4 Model, View, Projection;

		private float _farplane = 400.0f;

		private Vector3i _max = new Vector3i(0);
		private Vector3i _min = new Vector3i(0);

		public float FarPlane {
			get { return _farplane; }
			protected set {
				if (_farplane == value) { return; }
				_farplane = value;
				Projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(90.0f), 16f / 9f, 0.1f, _farplane);
			}
		}

		private Light _worldlight = new Light(0, 0, 0);

		public Light WorldLight {
			get { return _worldlight; }
			protected set { _worldlight = value; }
		}

		private Camera _camera = new Camera();

		protected Vector3 CameraPosition {
			get { return _camera.Position; }
			set { _camera.Position = value; }
		}

		private Shader _shader = new Shader("./Shaders/3D/basic.vert", "./Shaders/3D/basic.frag");
		private Shader _lightShader = new Shader("./Shaders/3D/basic.vert", "./Shaders/3D/light.frag");

		private List<PointLight> _lights = new List<PointLight>();

		protected Fog Fog = new Fog(new Vector3(0, 0, 0), 0.8f, 0.4f);

		private List<KeyValuePair<(int, int, int), Chunk>> _loadQ = new List<KeyValuePair<(int, int, int), Chunk>>();

		public World() { }

		public virtual void Build () { }
		protected virtual void Setup() { }

		protected void AddLight (PointLight light) {
			_lights.Add(light);
			if (_loaded) { light.Load(); }
		}

		protected void PlaceStructure (Structure str, int x, int z) {
			Chunk? found = null;

			bool[] pos = new bool[] {
				x < 0,
				z < 0
			};

			x += pos[0] ? 1 : 0;
			z += pos[1] ? 1 : 0;

			int cx = x / Chunk.Size + (pos[0] ? -1 : 0);
			int cz = z / Chunk.Size + (pos[1] ? -1 : 0);

			int px = pos[0] ? Chunk.Size - 1 - (Math.Abs(x) % Chunk.Size) : x % Chunk.Size;
			int pz = pos[1] ? Chunk.Size - 1 - (Math.Abs(z) % Chunk.Size) : z % Chunk.Size;

			int y = 0;

			for (int i = _max.Y; i >= _min.Y; i--) {
				if (_chunks.ContainsKey((cx, i, cz))) {
					found = _chunks[(cx, i, cz)];

					y = found.GetTopCube(px, pz);
					if (y == -1) {
						found = null;
					} else {
						y += (i * Chunk.Size);
						break;
					}
				}
			}

			if (found != null) {
				List<StructCube> add = str.Place(x, y, z);

				foreach (StructCube cube in add) {
					if (cube.CubeType == StructCubeType.Cube) {
						AddCube(cube.Type, cube.Position.X, cube.Position.Y, cube.Position.Z);
					} else {
						if (cube.Light == null) { continue; }
						LightDef def = ((LightDef) cube.Light);

						AddLight(new PointLight(def.Light, cube.Position, def.Distance));
					}
				}
			}
		}

		public void Init () {
			if (_disposed) {
				Error.Report("Trying to initialize a world that was previously disposed");
				return;
			}

			if (_initialized) { return; }

			Setup();
			Build();

			_initialized = true;
		}

		public void Load () {
			if (_disposed) {
				Error.Report("Trying to load a world that was previously disposed");
				return;
			}

			if (_loaded) { return; }

			Model = Matrix4.Identity;
			Projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(90.0f), 16f / 9f, 0.1f, _farplane);
			View = _camera.LookAt;

			foreach (PointLight light in _lights) {
				light.Load();
			}

			_shader.Load();
			_lightShader.Load();

			_loaded = true;
		}

		private void LoadChunk (KeyValuePair<(int, int, int), Chunk> pair) {
			int[][] edges = new int[6][];
			(int x, int y, int z) chunkPos = pair.Key;

			// left chunk, right face
			if (_chunks.ContainsKey((chunkPos.x - 1, chunkPos.y, chunkPos.z))) {
				edges[(int) Face.Left] = _chunks[(chunkPos.x - 1, chunkPos.y, chunkPos.z)].GetEdge(Face.Right);
			}

			// right chunk, left face
			if (_chunks.ContainsKey((chunkPos.x + 1, chunkPos.y, chunkPos.z))) {
				edges[(int) Face.Right] = _chunks[(chunkPos.x + 1, chunkPos.y, chunkPos.z)].GetEdge(Face.Left);
			}

			// top chunk, bottom face
			if (_chunks.ContainsKey((chunkPos.x, chunkPos.y + 1, chunkPos.z))) {
				edges[(int) Face.Top] = _chunks[(chunkPos.x, chunkPos.y + 1, chunkPos.z)].GetEdge(Face.Bottom);
			}

			// bottom chunk, top face
			if (_chunks.ContainsKey((chunkPos.x, chunkPos.y - 1, chunkPos.z))) {
				edges[(int) Face.Bottom] = _chunks[(chunkPos.x, chunkPos.y - 1, chunkPos.z)].GetEdge(Face.Top);
			}

			// front chunk, back face
			if (_chunks.ContainsKey((chunkPos.x, chunkPos.y, chunkPos.z + 1))) {
				edges[(int) Face.Front] = _chunks[(chunkPos.x, chunkPos.y, chunkPos.z + 1)].GetEdge(Face.Back);
			}

			// Back chunk, front face
			if (_chunks.ContainsKey((chunkPos.x, chunkPos.y, chunkPos.z - 1))) {
				edges[(int) Face.Back] = _chunks[(chunkPos.x, chunkPos.y, chunkPos.z - 1)].GetEdge(Face.Front);
			}

			pair.Value.Load(edges);
		}

		public void AddCube (int id, int x, int y, int z) {
			bool[] pos = new bool[] {
				x < 0,
				y < 0,
				z < 0
			};

			x += pos[0] ? 1 : 0;
			y += pos[1] ? 1 : 0;
			z += pos[2] ? 1 : 0;

			int cx = x / Chunk.Size + (pos[0] ? -1 : 0);
			int cy = y / Chunk.Size + (pos[1] ? -1 : 0);
			int cz = z / Chunk.Size + (pos[2] ? -1 : 0);

			if (!_chunks.ContainsKey((cx, cy, cz))) {
				_chunks.Add(
					(cx, cy, cz), 
					new Chunk(
						new Vector3(
							cx * Chunk.Size, 
							cy * Chunk.Size, 
							cz * Chunk.Size
						)
					)
				);

				if (cx > _max.X) { _max.X = cx; }
				if (cx < _min.X) { _min.X = cx; }

				if (cy > _max.Y) { _max.Y = cy; }
				if (cy > _max.Y) { _min.Y = cy; }
				
				if (cz > _max.Z) { _max.Z = cz; }
				if (cz > _max.Z) { _min.Z = cz; }
			}

			int px = pos[0] ? Chunk.Size - 1 - (Math.Abs(x) % Chunk.Size) : x % Chunk.Size;
			int py = pos[1] ? Chunk.Size - 1 - (Math.Abs(y) % Chunk.Size) : y % Chunk.Size;
			int pz = pos[2] ? Chunk.Size - 1 - (Math.Abs(z) % Chunk.Size) : z % Chunk.Size;

			_chunks[(cx, cy, cz)].AddCubes(id, px, py, pz);
		}

		public virtual void Update(FrameEventArgs args, MouseState mouse, KeyboardState state) {
			if (_disposed) { return; }

			_camera.Update(args.Time, mouse.Delta, state);
			_camera.Move(_camera.Speed);
			View = _camera.LookAt;

			// update which lights get rendered here


			// Load chunks
			foreach (KeyValuePair<(int, int, int), Chunk> pair in _chunks) {
				if (_loadQ.Contains(pair)) { continue; }

				float distance = pair.Value.GetDistance(_camera.Position);
				if (distance > _farplane + Chunk.Size) {
					pair.Value.Unload();
					continue; 
				}

				_loadQ.Add(pair);
			}

			if (_loadQ.Count > 0) {
				LoadChunk(_loadQ[0]);
				_loadQ.RemoveAt(0);
			}
		}

		public void Unload () {
			if (!_loaded) { return; }

			foreach (KeyValuePair<(int, int, int), Chunk> chunk in _chunks) {
				chunk.Value.Unload();
			}

			_loaded = false;
		}

		public void Dispose () {
			if (_disposed) { return; }

			OnDispose();

			_shader.Dispose();
			_lightShader.Dispose();

			foreach (KeyValuePair<(int, int, int), Chunk> chunk in _chunks) {
				chunk.Value.Dispose();
			}

			_chunks.Clear();

			_disposed = true;
		}

		protected virtual void OnDispose() {  }

		public void Render () {
			if (_disposed) { return; }

			GL.ClearColor(Fog.Colour.X, Fog.Colour.Y, Fog.Colour.Z, 1);
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

			RenderChunks();
			RenderLights();
		}

		private void RenderChunks () {
			_shader.SetMatrix4("view", View);
			_shader.SetMatrix4("model", Model);
			_shader.SetMatrix4("projection", Projection);

			_shader.SetVector3("viewPos", _camera.Position);

			_shader.SetVector3("global.direction", _worldlight.Direction);
			_shader.SetVector3("global.ambient", _worldlight.Ambient);
			_shader.SetVector3("global.diffuse", _worldlight.Diffuse);
			_shader.SetVector3("global.specular", _worldlight.Specular);

			_shader.SetFloat("farPlane", FarPlane);

			_shader.SetFloat("FogDensity", Fog.Density);
			_shader.SetFloat("FogStart", Fog.Start);
			_shader.SetFloat("FogEnd", Fog.End);
			_shader.SetVector3("FogColour", Fog.Colour);

			for (int i = 0; i < _lights.Count; i++) {
				_lights[i].AddToShader(i, _shader);
			}

			_shader.SetInt("lightCount", _lights.Count);

			_shader.Use();

			foreach (KeyValuePair<(int, int, int), Chunk> pair in _chunks) {
				float distance = pair.Value.GetDistance(_camera.Position);
				if (distance > _farplane + Chunk.Size) { continue; }

				if (distance > Chunk.Size * 2) {
					float angle = pair.Value.GetAngle(_camera.Position, _camera.GetViewDirection());
					if (angle > Math.PI / 2) { continue; }
				}

				pair.Value.Render();
			}
		}

		private void RenderLights () {
			_lightShader.SetMatrix4("view", View);
			_lightShader.SetMatrix4("model", Model);
			_lightShader.SetMatrix4("projection", Projection);

			_lightShader.SetVector3("viewPos", _camera.Position);

			_lightShader.SetFloat("farPlane", FarPlane);

			_lightShader.SetFloat("FogDensity", Fog.Density);
			_lightShader.SetFloat("FogStart", Fog.Start);
			_lightShader.SetFloat("FogEnd", Fog.End);
			_lightShader.SetVector3("FogColour", Fog.Colour);

			_lightShader.Use();

			for (int i = 0; i < _lights.Count; i++) {
				_lights[i].Render(_lightShader);
			}
		}
	}
}
