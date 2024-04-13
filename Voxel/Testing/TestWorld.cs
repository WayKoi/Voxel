using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voxel.Components;
using Voxel.Structs;

namespace Voxel.Testing {
	internal class TestWorld : World {

		private int type = 0;

		private Structure _rod = new Structure("./Structures/test.struct");

		protected override void Setup() {
			base.Setup();

			Fog = new Fog(new Vector3(0.7f, 0.4f, 0.2f), 1, 0.05f);

			type = Cube.AddType(new StaticCubeType(new Vector3(0.5f, 1, 0.5f), 1, 1));
		}

		public override void Build() {
			base.Build();

			for (int x = -256; x < 256; x++) {
				for (int z = -256; z < 256; z++) {
					int y = 16 + (int) Math.Round(8 * Math.Sin(x / (Math.PI * 4)) + 6 * Math.Cos(z / (Math.PI * 4)));

					for (int h = y; h >= 0; h--) {
						AddCube(type, x, h, z);
					}
				}
			}

			Random rand = new Random();

			for (int i = 0; i < 100; i++) {
				AddLight(
					new PointLight(
						new Vector3(
							1
						/*(float) rand.NextDouble(),
						(float) rand.NextDouble(),
						(float) rand.NextDouble()*/
						),
						new Vector3(
							rand.Next(-256, 256),
							rand.Next(10, 40),
							rand.Next(-256, 256)
						),
						rand.Next(30, 60)
					)
				);
			}

			for (int i = 0; i < 100; i++) {
				PlaceStructure(
					_rod, 
					rand.Next(-200, 200),
					rand.Next(-200, 200)
				);
			}
		}
	}
}
