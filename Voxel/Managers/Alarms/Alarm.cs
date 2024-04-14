using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voxel.Managers.Alarms {
	public class Alarm {
		private int _timer = 0;
		private int _count = 0;

		private Action _ring = Dummy;

		public void Tick () {
			_count++;

			if (_count > _timer) {
				_ring();
			}
		}

		private static void Dummy () { }
	}
}
