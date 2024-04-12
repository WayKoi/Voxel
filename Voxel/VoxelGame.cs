﻿using OpenTK.Graphics.OpenGL4;
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
using Voxel.Testing;

namespace Voxel {
	internal class VoxelGame : GameWindow {
		private TestWorld world = new TestWorld();

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

			world.Render();

			SwapBuffers();
		}

		protected override void OnUpdateFrame(FrameEventArgs args) {
			base.OnUpdateFrame(args);

			MouseState state = MouseState.GetSnapshot();
			KeyboardState input = KeyboardState;

			world.Update(args, state, input);

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

			world.Init();
			world.Load();
		}

		protected override void OnResize(ResizeEventArgs e) {
			base.OnResize(e);

			GL.Viewport(0, 0, e.Width, e.Height);
		}
	}
}
