using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voxel {
	internal static class Error {
		private static string _path = Environment.CurrentDirectory + "\\logs\\";

		private static string _current = "";
		private static bool _init = false;

		public static void Report (string message) {
			if (!_init) { Init(); }
			if (!File.Exists(_current)) { Console.Error.WriteLine("Log file for errors did not exist when trying to write Error :\n" + message); }

			List<string> lines = File.ReadAllLines(_current).ToList();
			lines.Add(message);
			File.WriteAllLines(_current, lines);
		}

		private static void Init () {
			if (_init) { return; }
			
			if (!Directory.Exists(_path)) {
				Directory.CreateDirectory(_path);
			}

			string filename = _path + DateTime.Now.ToString("dddd-MMMM-dd-yyyy-HH:mm:ss") + ".log";

			if (!File.Exists(filename)) {
				File.Create(filename).Close();
			}

			_current = filename;
			_init = true;
		}

	}
}
