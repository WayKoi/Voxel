using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voxel.Structs {
	internal struct Fog {
		public Vector3 Colour;
		public float Density, Start, End;

		public Fog (Vector3 colour, float density, float start, float end = 0.9f) {
			Colour = colour;
			Density = density;
			Start = Math.Clamp(start, 0, 1);
			End = Math.Clamp(end, 0, 1);
		}

		public Fog(float colour, float density, float start, float end = 0.9f) {
			Colour = new Vector3(colour);
			Density = density;
			Start = Math.Clamp(start, 0, 1);
			End = Math.Clamp(end, 0, 1);
		}

		private static Dictionary<string, Fog> _table = new Dictionary<string, Fog>();
		private static Fog _default = new Fog(new Vector3(0.8f, 0.8f, 0.9f), 0.8f, 0.4f);

		public static void AddFog(string name, Fog fog) {
			name = Standardize(name);

			if (_table.ContainsKey(name)) {
				Error.Report("Overwrote existing fog : " + name);
				_table.Remove(name);
			}

			_table.Add(name, fog);
		}

		public static Fog GetFog(string name) {
			name = Standardize(name);

			if (_table.ContainsKey(name)) {
				return _table[name];
			}

			Error.Report("Attempted to get a fog that is not registered : " + name);
			return _default;
		}

		private static string Standardize(string name) {
			return name.Replace("\n", "").Replace("\t", "");
		}

	}
}
