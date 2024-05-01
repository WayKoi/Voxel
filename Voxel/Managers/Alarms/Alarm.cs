using OpenTK.Windowing.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voxel.Managers.Alarms {
	public abstract class Alarm {
		private Action _ring = Dummy;
		public Action Ring {
			get { return _ring; }
			protected set { _ring = value; }
		}

		private bool _reset = false;
		public bool Reset {
			get { return _reset; }
		}

		private bool _rung = false;
		public bool Rung {
			get { return _rung; }
			protected set { _rung = value; }
		}

		public Alarm (Action ring, bool reset = false) {
			_ring = ring;
			_reset = reset;
		}

		public abstract void Tick (FrameEventArgs args);

		public virtual void ResetAlarm() {
			_rung = false;
		}

		private static void Dummy () { }
	}

	public class GameTimeAlarm : Alarm {
		private int _ringtime = 0, _timer = 0;

		public GameTimeAlarm(Action ring, int ringtime, bool reset = false) : base(ring, reset) {
			_ringtime = ringtime;
		}

		public override void Tick(FrameEventArgs args) {
			if (Rung) { return; }

			_timer++;
			if (_timer >= _ringtime) {
				Ring();
				Rung = true;
			}
		}

		public override void ResetAlarm() {
			base.ResetAlarm();
			_timer = 0;
		}
	}

	public class RealTimeAlarm : Alarm {
		private double _ringtime = 0, _timer = 0;

		public RealTimeAlarm(Action ring, float ringtime, bool reset = false) : base(ring, reset) {
			_ringtime = ringtime;
		}

		public override void Tick(FrameEventArgs args) {
			if (Rung) { return; }

			_timer += args.Time;
			if (_timer >= _ringtime) {
				Ring();
				Rung = true;
			}
		}

		public override void ResetAlarm() {
			base.ResetAlarm();
			_timer = 0;
		}
	}

	public class ConditionalAlarm : Alarm {
		private Func<bool> _check;

		public ConditionalAlarm(Action ring, Func<bool> condition, bool reset = false) : base(ring, reset) {
			_check = condition;
		}

		public override void Tick(FrameEventArgs args) {
			if (Rung) { return; }

			if (_check()) {
				Ring();
				Rung = true;
			}
		}
	}
}
