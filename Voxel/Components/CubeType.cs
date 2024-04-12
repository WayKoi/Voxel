using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Voxel.Components {
	public class CubeType {
		private float _alpha = 1;

		public float Alpha {
			get { return _alpha; }
			set { _alpha = value; }
		}

		private float _shiny = 0;

		public float Shiny {
			get { return _shiny; }
			private set { _shiny = value; }
		}

		public CubeType (float alpha, float shiny) {
			Alpha = alpha;
			Shiny = shiny;
		}

		public virtual Vector4 GetColour (float x, float y, float z) {
			return new Vector4(1, 1, 1, Alpha);
		}
	}

	public class StaticCubeType : CubeType {
		private Vector3 _colour = new Vector3(1);

		public Vector4 Colour {
			get { return new Vector4(_colour, Alpha); }
		}

		public StaticCubeType(Vector3 colour, float alpha, float shiny) : base(alpha, shiny) {
			_colour = colour;
		}

		public override Vector4 GetColour (float x, float y, float z) {
			return Colour;
		}
	}	
}
