using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voxel.Components {
	public abstract class Component {
		private Vector3 _position, _size, _origin, _velocity;

		public Vector3 Origin {
			get { return _origin; }
		}

		public Vector3 Size {
			get { return _size; }
		}

		public Vector3 Position { 
			get { return _position; } 
			set {
				if (_position == value) { return; }
				_position = value;
				OnPositionChanged();
			} 
		}

		public float X {
			get { return _position.X; } 
			set {
				if (_position.X == value) { return; }
				_position.X = value;
				OnPositionChanged();
			}
		}

		public float Y {
			get { return _position.Y; }
			set {
				if (_position.Y == value) { return; }
				_position.Y = value;
				OnPositionChanged();
			}
		}

		public float Z {
			get { return _position.Z; }
			set {
				if (_position.Z == value) { return; }
				_position.Z = value;
				OnPositionChanged();
			}
		}

		/// <summary>
		/// Velocity in voxels per second
		/// </summary>
		public Vector3 Velocity {
			get { return _velocity; }
			set {
				if (_velocity == value) { return; }
				_velocity = value;
			}
		}

		public float VX {
			get { return _velocity.X; }
			set {
				if (_velocity.X == value) { return; }
				_velocity.X = value;
			}
		}

		public float VY {
			get { return _velocity.Y; }
			set {
				if (_velocity.Y == value) { return; }
				_velocity.Y = value;
			}
		}

		public float VZ {
			get { return _velocity.Z; }
			set {
				if (_velocity.Z == value) { return; }
				_velocity.Z = value;
			}
		}

		public Component (Vector3 size, Vector3? origin = null) {
			_origin = Vector3.Zero;
			if (origin != null) {
				_origin = (Vector3) origin;
			}

			_size = size;
		}

		public virtual void Load () { }

		public virtual void Update(FrameEventArgs args, MouseState mouse, KeyboardState keys) {
			// Update the position of the object via velocity
			double gametime = args.Time;

			X += (float) (VX * gametime);
			Y += (float) (VY * gametime);
			Z += (float) (VZ * gametime);
		}

		public virtual void Render () {  }

		public event EventHandler? PositionChanged;

		private void OnPositionChanged() {
			EventHandler? handler = PositionChanged;
			if (handler != null) {
				handler(this, EventArgs.Empty);
			}
		}
	}
}
