using OpenTK.Windowing.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voxel.Managers.Alarms {
	internal class AlarmGroup {
		private List<Alarm> _alarms = new List<Alarm>();
		private int _count = 0;

		private bool _paused = false, _disposed = false;

		public bool Disposed {
			get { return _disposed; }
		}

		public bool Paused {
			get { return _paused; }
			set { _paused = value; }
		}

		public void Add(Alarm alarm) {
			if (_disposed) { 
				Error.Report("Tried to add alarm to disposed AlarmGroup");
				return;
			}

			_alarms.Add(alarm);
			_count++;
		}

		public void Tick (FrameEventArgs args) {
			if (_disposed) { return; }
			if (_paused) { return; }

			for (int i = 0; i < _count; i++) {
				_alarms[i].Tick(args);
				
				if (_alarms[i].Rung) {
					if (_alarms[i].Reset) {
						_alarms[i].ResetAlarm();
					} else {
						_alarms.RemoveAt(i);
						i--;
						_count--;
					}
				}
			}
		}

		public void Dispose () {
			if (_disposed) { return; }

			_alarms.Clear();
			_count = 0;

			_disposed = true;
		}
	}
}
