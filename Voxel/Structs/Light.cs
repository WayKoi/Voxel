using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voxel.Structs {
	internal struct Light {
		public Vector3 Ambient;
		public Vector3 Diffuse;
		public Vector3 Specular;
		public Vector3 Direction;

		public Light (Vector3 ambient, Vector3 diffuse, Vector3 specular, Vector3? direction = null) {
			Ambient  = ambient;
			Diffuse  = diffuse;
			Specular = specular;

			Direction = new Vector3(0, -1, 0);
			if (direction != null) {
				Direction = (Vector3) direction;
			}

			Direction.Normalize();
		}

		public Light (float ambient, float diffuse, float specular, Vector3? direction = null) {
			Ambient  = new Vector3(ambient);
			Diffuse  = new Vector3(diffuse);
			Specular = new Vector3(specular);

			Direction = new Vector3(0, -1, 0);
			if (direction != null) {
				Direction = (Vector3) direction;
			}

			Direction.Normalize();
		}

		private static Dictionary<string, Light> _lights = new Dictionary<string, Light>();
		private static Light _default = new Light(0.5f, 0, 0);

		public static void AddLight (string name, Light light) {
			name = Standardize(name);

			if (_lights.ContainsKey(name)) {
				Error.Report("Overwrote existing light : " + name);
				_lights.Remove(name);
			}

			_lights.Add(name, light);
		}

		public static Light GetLight (string name) {
			name = Standardize(name);

			if (_lights.ContainsKey(name)) {
				return _lights[name];
			}

			Error.Report("Attempted to get a light that is not registered : " + name);
			return _default;
		}

		private static string Standardize(string name) {
			return name.Replace("\n", "").Replace("\t", "");
		}
	}
}
