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
using static OpenTK.Graphics.OpenGL.GL;

namespace Voxel {
	internal class VoxelGame : GameWindow {
		private List<Cube> cube = new List<Cube>();
		private List<int> VAOs = new List<int>(), VBOs = new List<int>();
		private Camera Cam = new Camera();

		private Matrix4 Model, View, Projection;

		private Shader _shader = new Shader("./Shaders/3D/basic.vert", "./Shaders/3D/basic.frag");

		public VoxelGame(int width, int height, string title) : 
			base(
				GameWindowSettings.Default,
				new NativeWindowSettings() {
					ClientSize = (width, height),
					Title = title
				}
			) {
		}

		protected override void OnRenderFrame(FrameEventArgs args) {
			base.OnRenderFrame(args);

			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

			// render stuff here
			
			_shader.SetMatrix4("view", View);
			_shader.SetMatrix4("model", Model);
			_shader.SetMatrix4("projection", Projection);

			_shader.Use();

			for (int i = 0; i < cube.Count; i++) {
				GL.BindVertexArray(VAOs[i]);
				GL.DrawArrays(PrimitiveType.Triangles, 0, 6 * 6);
				GL.BindVertexArray(0);
			}

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

			Random rand = new Random();

			for (int i = 0; i < 10000; i++) {
				cube.Add(
					new Cube(
						new Vector4(
							(float) rand.NextDouble(), 
							(float) rand.NextDouble(), 
							(float) rand.NextDouble(), 
							1
						),

						new Vector3(
							rand.Next(-40, 40), 
							rand.Next(-40, 40), 
							rand.Next(-40, 40)
						)
					)
				);

				Vertex3D[] verts = cube[i].GetVerts();

				VBOs.Add(Vertex3D.GenVBO(verts));
				Vertex3D.BufferVertices(VBOs[i], verts);
				VAOs.Add(Vertex3D.GenVAO(VBOs[i]));
			}

			_shader.Load();
		}

		protected override void OnResize(ResizeEventArgs e) {
			base.OnResize(e);

			GL.Viewport(0, 0, e.Width, e.Height);
		}
	}
}
