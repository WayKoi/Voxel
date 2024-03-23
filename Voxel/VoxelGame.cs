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
		private List<Cube> cube = new List<Cube>();
		private List<PointLight> lights = new List<PointLight>();

		private Camera Cam = new Camera();

		private Chunk chunk = new Chunk(0, 0, 0);

		private Matrix4 Model, View, Projection;

		private Shader _shader = new Shader("./Shaders/3D/basic.vert", "./Shaders/3D/basic.frag");
		private Shader _lightShader = new Shader("./Shaders/3D/basic.vert", "./Shaders/3D/light.frag");

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

			
			for (int i = 0; i < lights.Count; i++) {
				lights[i].AddToShader(i, _shader);
			}

			_shader.SetInt("lightCount", lights.Count);

			_shader.Use();

			chunk.Render();

			_lightShader.Use();

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

			CursorState = CursorState.Grabbed;

			Model = Matrix4.Identity;
			Projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(90.0f), 16f / 9f, 0.1f, 100.0f);
			View = Cam.LookAt;

			chunk.Load();

			Random rand = new Random();

			List<Vector3> Pos = new List<Vector3>();

			for (int i = 0; i < 32; i++) {
				for (int ii = 0; ii < 32; ii++) {
					for (int iii = 0; iii < 32; iii++) {
						cube.Add(
							new Cube(
								new Vector4(1
									/*(float) rand.NextDouble(),
									(float) rand.NextDouble(),
									(float) rand.NextDouble(),
									(float) rand.NextDouble()*/
								)
							)
						);

						Pos.Add(new Vector3(i, ii, iii));
					}
				}
			}

			/*for (int i = 0; i < 500; i++) {
				cube.Add(
					new Cube(
						new Vector4(1
							*//*(float) rand.NextDouble(),
							(float) rand.NextDouble(),
							(float) rand.NextDouble(),
							(float) rand.NextDouble()*//*
						)
					)
				);

				Pos.Add(
					new Vector3(
						rand.Next(0, 32),
						rand.Next(0, 32),
						rand.Next(0, 32)
					)
				);
			}*/

			chunk.Add(cube.ToArray(), Pos.ToArray());

			for (int i = 0; i < 100; i++) {	
				lights.Add(
					new PointLight(
						new Vector3(
							(float) rand.NextDouble(),
							(float) rand.NextDouble(),
							(float) rand.NextDouble()
						),
						lightPos()
						/*new Vector3(
							rand.Next(-4, 37),
							rand.Next(-4, 37),
							rand.Next(-4, 37)
						)*/,
						rand.Next(10, 20)
					)
				);

				lights[i].Load();
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
