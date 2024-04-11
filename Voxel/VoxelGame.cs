using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Voxel.Components;
using Voxel.Shaders;
using Voxel.Structs;

namespace Voxel {
	internal class VoxelGame : GameWindow {
		private List<PointLight> lights = new List<PointLight>();

		private Camera Cam = new Camera();

		// private Chunk chunk = new Chunk(new Vector3(0));
		private World world = new World();

		private Matrix4 Model, View, Projection;

		private Shader _shader = new Shader("./Shaders/3D/basic.vert", "./Shaders/3D/basic.frag");
		private Shader _lightShader = new Shader("./Shaders/3D/basic.vert", "./Shaders/3D/light.frag");

		private float FarPlane = 400.0f;

		public VoxelGame(int width, int height, string title) : 
			base(
				GameWindowSettings.Default,
				new NativeWindowSettings() {
					ClientSize = (width, height),
					Title = title
				}
			) {

			UpdateFrequency = 120;
		}

		protected override void OnRenderFrame(FrameEventArgs args) {
			base.OnRenderFrame(args);

			GL.ClearColor(0.8f, 0.8f, 0.9f, 1);
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

			// render stuff here
			
			_shader.SetMatrix4("view", View);
			_shader.SetMatrix4("model", Model);
			_shader.SetMatrix4("projection", Projection);

			_shader.SetVector3("viewPos", Cam.Position);

			_lightShader.SetMatrix4("view", View);
			_lightShader.SetMatrix4("model", Model);
			_lightShader.SetMatrix4("projection", Projection);

			_shader.SetVector3("global.direction", Vertex3D.Normalize(new Vector3(-1, -1, -1)));
			_shader.SetVector3("global.ambient", new Vector3(0.1f));
			_shader.SetVector3("global.diffuse", new Vector3(0.0f));
			_shader.SetVector3("global.specular", new Vector3(0.0f));
			_shader.SetFloat("farPlane", FarPlane);
			_shader.SetFloat("FogDensity", 0.8f);
			_shader.SetFloat("FogStart", 0.4f);
			_shader.SetVector3("FogColour", new Vector3(0.8f, 0.8f, 0.9f));

			for (int i = 0; i < lights.Count; i++) {
				lights[i].AddToShader(i, _shader);
			}

			_shader.SetInt("lightCount", lights.Count);

			_shader.Use();

			world.Render();

			_lightShader.Use();

			// _lightShader.SetVector3("lightColour", new Vector3(1));

			for (int i = 0; i < lights.Count; i++) {
				lights[i].Render(_lightShader);
			}

			
			/*for (int i = 0; i < cube.Count; i++) {
				GL.BindVertexArray(VAOs[i]);
				GL.DrawArrays(PrimitiveType.Triangles, 0, 6 * 6);
				GL.BindVertexArray(0);
			}*/

			SwapBuffers();
		}

		protected override void OnUpdateFrame(FrameEventArgs args) {
			base.OnUpdateFrame(args);

			MouseState state = MouseState.GetSnapshot();

			Cam.Update(args.Time, state.Delta, KeyboardState.GetSnapshot());
			Cam.Move(Cam.Speed);
			View = Cam.LookAt;

			KeyboardState input = KeyboardState;
			if (input.IsKeyDown(Keys.Escape)) {
				Close();
			}
		}

		protected override void OnLoad() {
			base.OnLoad();

			GL.Enable(EnableCap.DepthTest);
			GL.Enable(EnableCap.CullFace);
			GL.Enable(EnableCap.Blend);
			GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
			// GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);

			CursorState = CursorState.Grabbed;

			Model = Matrix4.Identity;
			Projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(90.0f), 16f / 9f, 0.1f, FarPlane);
			View = Cam.LookAt;

			for (int i = 0; i < 40; i++) {
				world.AddCube(0, 0, i, 0);
			}

			world.AddCube(0, -1, 0, 0);
			world.AddCube(0, -2, 0, 0);
			world.AddCube(0, -2, 1, 0);
			world.AddCube(0, -1, 1, 0);

			world.AddCube(0, -1, 0, 1);
			world.AddCube(0, -2, 0, 1);
			world.AddCube(0, -2, 1, 1);
			world.AddCube(0, -1, 1, 1);

			/*for (int x = 0; x < 32; x++) {
				for (int z = 0; z < 32; z++) {
					int y = 16 + (int) Math.Round(8 * Math.Sin((x) / (Math.PI * 4)) + 6 * Math.Cos((z) / (Math.PI * 4)));

					for (int h = y; h >= 0; h--) {
						world.AddCube(0, x, h, z);
					}
				}
			}*/

			/*for (int x = -256; x < 256; x++) {
				for (int z = -256; z < 256; z++) {
					int y = 16 + (int) Math.Round(8 * Math.Sin(x / (Math.PI * 4)) + 6 * Math.Cos(z / (Math.PI * 4)));

					for (int h = y; h >= 0; h--) {
						world.AddCube(0, x, h, z);
					}
				}
			}*/

			world.Init();

			Random rand = new Random();

			List<Vector3i> pos = new List<Vector3i>();

			int offsetx = rand.Next(0, 30);
			int offsetz = rand.Next(0, 30);

			/*for (int x = -256; x < 256; x++) {
				for (int z = -256; z < 256; z++) {
					int y = (int) Math.Round(8 * Math.Sin((x + offsetx) / (Math.PI * 4)) + 6 * Math.Cos((z + offsetz) / (Math.PI * 4)));

					for (int h = y; h >= -20; h--) {
						cube.Add(new Cube(new Vector4(1, 1, 1, 1)));
						pos.Add(new Vector3i(x, h, z));
					}
				}
			}

			world.AddCubes(pos, cube);*/

			// lights.Add(new PointLight(new Vector3((float) (255 / 255.0), (float) (214 / 255.0), (float) (170 / 255.0)), new Vector3(16, 35, 16), 100));

			/*lights.Add(new PointLight(new Vector3(0f, 0.9f, 0f), new Vector3(-5, 16, 16), 100));
			
			lights.Add(new PointLight(new Vector3(0.9f, 0f, 0f), new Vector3(16, 16, -5), 100));*/

			/*lights.Add(new PointLight(new Vector3(0.9f, 0.9f, 0f), new Vector3(-1, -1, -1), 5));
			lights.Add(new PointLight(new Vector3(0.9f, 0.9f, 0f), new Vector3(-1, -3, -1), 5));
			lights.Add(new PointLight(new Vector3(0.9f, 0.9f, 0f), new Vector3(-1, -5, -1), 5));
			lights.Add(new PointLight(new Vector3(0.9f, 0.9f, 0f), new Vector3(-1, -7, -1), 5));
			lights.Add(new PointLight(new Vector3(0.9f, 0.9f, 0f), new Vector3(-1, -9, -1), 5));
			lights.Add(new PointLight(new Vector3(0.9f, 0.9f, 0f), new Vector3(-1, -11, -1), 5));
			lights.Add(new PointLight(new Vector3(0.9f, 0.9f, 0f), new Vector3(-1, -13, -1), 5));
			lights.Add(new PointLight(new Vector3(0.9f, 0.9f, 0f), new Vector3(-1, -15, -1), 5));
			lights.Add(new PointLight(new Vector3(0.9f, 0.9f, 0f), new Vector3(-1, -17, -1), 5));
			lights.Add(new PointLight(new Vector3(0.9f, 0.9f, 0f), new Vector3(-1, -19, -1), 5));
			lights.Add(new PointLight(new Vector3(0.9f, 0.9f, 0f), new Vector3(-1, -21, -1), 5));
			lights.Add(new PointLight(new Vector3(0.9f, 0.9f, 0f), new Vector3(-1, -23, -1), 5));
			lights.Add(new PointLight(new Vector3(0.9f, 0.9f, 0f), new Vector3(-1, -25, -1), 5));
			lights.Add(new PointLight(new Vector3(0.9f, 0.9f, 0f), new Vector3(-1, -27, -1), 5));
			lights.Add(new PointLight(new Vector3(0.9f, 0.9f, 0f), new Vector3(-1, -29, -1), 5));
			lights.Add(new PointLight(new Vector3(0.9f, 0.9f, 0f), new Vector3(-1, -31, -1), 5));
*/

			lights.Add(new PointLight(new Vector3(0.9f, 0.9f, 0.9f), new Vector3(-1, -1, -1), 5));
			lights.Add(new PointLight(new Vector3(0.9f, 0.9f, 0f), new Vector3(2, 2, 2), 5));
			lights.Add(new PointLight(new Vector3(0.9f, 0.9f, 0f), new Vector3(-1, 32, -1), 5));

			/*for (int i = 0; i < 100; i++) {
				lights.Add(
					new PointLight(
						new Vector3(
							(float) rand.NextDouble(),
							(float) rand.NextDouble(),
							(float) rand.NextDouble()
						),
						*//*lightPos()*//*
						new Vector3(
							rand.Next(-256, 256),
							rand.Next(10, 40),
							rand.Next(-256, 256)
						),
						rand.Next(30, 60)
					)
				);

				lights[i].Load();
			}*/

			foreach (PointLight light in lights) {
				light.Load();
			}

			_shader.Load();
			_lightShader.Load();
		}


		private Vector3 lightPos () {
			Random rand = new Random();

			float x = 0, y = 0, z = 0;

			int special = rand.Next(0, 4);

			if (special == 0) {
				// x point
				if (rand.Next(0, 2) == 0) {
					x = rand.Next(-10, -1);
				} else {
					x = rand.Next(33, 43);
				}
			} else {
				x = rand.Next(-10, 43);
			}

			if (special == 1) {
				// y point
				if (rand.Next(0, 2) == 0) {
					y = rand.Next(-10, -1);
				} else {
					y = rand.Next(33, 43);
				}
			} else {
				y = rand.Next(-10, 43);
			}

			if (special == 2) {
				// z point
				if (rand.Next(0, 2) == 0) {
					z = rand.Next(-10, -1);
				} else {
					z = rand.Next(33, 43);
				}
			} else {
				z = rand.Next(-10, 43);
			}

			return new Vector3(x, y, z);
		}


		protected override void OnResize(ResizeEventArgs e) {
			base.OnResize(e);

			GL.Viewport(0, 0, e.Width, e.Height);
		}
	}
}
