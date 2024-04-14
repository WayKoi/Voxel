using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voxel.Components;

namespace Voxel.Managers {
	public static class WorldM {
		private static Dictionary<string, World> _worlds = new Dictionary<string, World>();

		private static World? _current = null;
		private static string _currentName = string.Empty;

		public static void AddWorld (string name, World world) {
			if (name.Equals(string.Empty)) {
				Error.Report("Unable to add a world without a name to WorldM");
				return;
			}

			bool reload = false;

			if (_worlds.ContainsKey(name)) {
				if (_currentName.Equals(name)) { reload = true; _currentName = string.Empty; _current = null; }
				_worlds[name].Dispose();
				_worlds.Remove(name);
			}

			_worlds.Add(name, world);
			if (reload) { SetCurrent(name); }
		}

		public static void SetCurrent (string? name) {
			if (_currentName.Equals(name)) { return; }

			if (_current != null) {
				_current.Unload();
			}

			if (name == null) {
				_current = null;
				return;
			}

			if (_worlds.ContainsKey(name)) {
				_current = _worlds[name];
				_currentName = name;

				_current.Init();
				_current.Load();
			}
		}

		public static void RemoveWorld (string name) {
			if (_worlds.ContainsKey(name)) {
				if (_current == _worlds[name]) { _current = null; }
				_worlds[name].Dispose();
				_worlds.Remove(name);
			}
		}

		// Run the world
		public static void Render () {
			if (_current == null) { return; }

			_current.Render();
		}

		public static void Update (FrameEventArgs args, MouseState mouse, KeyboardState keyboard) {
			if (_current == null) { return; }

			_current.Update(args, mouse, keyboard);
		}
	}
}
