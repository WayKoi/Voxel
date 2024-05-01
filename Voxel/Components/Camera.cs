using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voxel.Components {
	public class Camera {
		private Vector3 _position;
		private Vector3 _rotation; // X = rot around Y, Y = up / down look (pitch), Z = roll
		private Matrix4 _lookat;

		public Vector3 Position {
			get { return _position; }
			set {
				_position = value;
				UpdateLookAt();
			}
		}

		public Vector3 Speed, Acceleration = new Vector3(0, 0, 0);
		private float MoveSpeed = 10;
		private float MaxSpeed = 20;

		public Vector3 Rotation {
			get { return _rotation; }
			set {
				_rotation = value;
				UpdateLookAt();
			}
		}

		public Matrix4 LookAt { get { return _lookat; } }

		public Camera() {
			_position = new Vector3(0f, 0f, 3f);

			UpdateLookAt();
		}

		public Vector3 GetViewDirection () {
			Vector3 view = new Vector3(
				(float) Math.Cos(Rotation.X + MathHelper.PiOver2),
				(float) Math.Tan(-Rotation.Y),
				(float) Math.Sin(Rotation.X + MathHelper.PiOver2)
			);

			view.Normalize();

			return view;
		}

		private void UpdateLookAt() {
			Vector3 offset = new Vector3(
				(float) Math.Cos(Rotation.X + MathHelper.PiOver2),
				(float) Math.Tan(-Rotation.Y),
				(float) Math.Sin(Rotation.X + MathHelper.PiOver2)
			);

			_lookat = Matrix4.LookAt(_position, _position + offset, new Vector3(0, 1, 0));
		}

		public void Update(double time, Vector2 state, KeyboardState keyState) {

			/*Vector3 move = Vector3.Zero;

			move.Z = (keyState.IsKeyDown(Keys.W) ? 1 : 0) + (keyState.IsKeyDown(Keys.S) ? -1 : 0);
			move.X = (keyState.IsKeyDown(Keys.A) ? 1 : 0) + (keyState.IsKeyDown(Keys.D) ? -1 : 0);
			move.Y = (keyState.IsKeyDown(Keys.Space) ? 1 : 0) + (keyState.IsKeyDown(Keys.LeftShift) ? -1 : 0);

			if (move != Vector3.Zero) {
				move.Normalize();

				move *= (float) (time * MoveSpeed);

				Matrix4 rotate = Matrix4.CreateRotationY(-Rotation.X);

				Vector3 movement = new Vector3(move.X, move.Y, move.Z);
				movement = Vector3.TransformVector(movement, rotate);

				Speed = movement;
			} else {
				Speed.X -= Speed.X / 25;
				if (Math.Abs(Speed.X) < 0.00001) {
					Speed.X = 0;
				}

				Speed.Z -= Speed.Z / 25;
				if (Math.Abs(Speed.Z) < 0.00001) {
					Speed.Z = 0;
				}

				Speed.Y -= Speed.Y / 25;
				if (Math.Abs(Speed.Y) < 0.00001) {
					Speed.Y = 0;
				}
			}

			Speed += (float) time * Acceleration;

			Speed.X = Math.Min(Math.Abs(Speed.X), MaxSpeed) * Math.Sign(Speed.X);
			Speed.Y = Math.Min(Math.Abs(Speed.Y), MaxSpeed) * Math.Sign(Speed.Y);
			Speed.Z = Math.Min(Math.Abs(Speed.Z), MaxSpeed) * Math.Sign(Speed.Z);*/

			if (state.X != 0 || state.Y != 0) {
				float dx = state.X;
				float dy = state.Y;

				float[] Conv = { 20, 15 };

				_rotation = new Vector3(
					(float) (_rotation.X + MathHelper.DegreesToRadians(dx / Conv[0]) + MathHelper.TwoPi) % MathHelper.TwoPi,
					(float) Math.Clamp(_rotation.Y + MathHelper.DegreesToRadians(dy / Conv[1]), MathHelper.DegreesToRadians(-85), MathHelper.DegreesToRadians(85)),
					0);

				UpdateLookAt();
			}
		}

		public void Move(Vector3 Amt) {
			if (Amt != Vector3.Zero) {
				_position += Amt;
				UpdateLookAt();
			}
		}
	}
}
