using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Platform.Windows;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voxel.Structs;

namespace Voxel.Components {
	public class Player : Component {
		private Camera _camera;

		private CameraType _cameraType = CameraType.FirstPerson;

		private int _vbo, _vao;

		public Player(Camera cam) : base(new Vector3(1)) {
			_camera = cam;

			Position = new Vector3(0, 40, 0);

			PositionChanged += Player_PositionChanged;
		}

		private void Player_PositionChanged(object? sender, EventArgs e) {
			Vertex3D.BufferVertices(_vbo, Cube.GetVerts(Position - Origin, new Vector4(0.2f, 1f, 0.2f, 1)));
		}

		public override void Load() {
			base.Load();

			_vbo = Vertex3D.GenVBO(Cube.GetVerts(Position - Origin, new Vector4(0.2f, 1f, 0.2f, 1)));
			_vao = Vertex3D.GenVAO(_vbo);
		}

		public override void Update(FrameEventArgs args, MouseState mouse, KeyboardState keys) {
			base.Update(args, mouse, keys);

			Vector4 Move = new Vector4(0, 0, 0, 1);

			if (keys.IsKeyDown(Keys.A)) {
				Move.X += 5;
			}

			if (keys.IsKeyDown(Keys.D)) {
				Move.X -= 5;
			}

			if (keys.IsKeyDown(Keys.W)) {
				Move.Z += 5;
			}

			if (keys.IsKeyDown(Keys.S)) {
				Move.Z -= 5;
			}

			Matrix4 rotate = Matrix4.CreateRotationY(-_camera.Rotation.X);

			Move *= rotate;

			VX = Move.X;
			VZ = Move.Z;

			if (keys.IsKeyPressed(Keys.Space)) {
				VY += 7;
			}

			switch (_cameraType) {
				case CameraType.FirstPerson:
					_camera.Position = Position - Origin + Size / 2;
					break;
				case CameraType.ThirdPerson:
					_camera.Position = Position - Origin + Size / 2 + (_camera.GetViewDirection() * -5);
					break;
			}
		}

		public override void Render() {
			base.Render();

			GL.BindVertexArray(_vao);
			GL.DrawArrays(PrimitiveType.Triangles, 0, 6 * 6);
			GL.BindVertexArray(0);
		}
	}

	public enum CameraType {
		FreeRoam,
		FirstPerson,
		ThirdPerson
	}
}
