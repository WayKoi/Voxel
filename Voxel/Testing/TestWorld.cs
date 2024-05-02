using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voxel.Components;
using Voxel.Managers.Alarms;
using Voxel.Structs;

namespace Voxel.Testing {
	internal class TestWorld : World {

		private int type = 0;

		private Structure _rod = new Structure("./Structures/test.struct");

		protected override void Setup() {
			base.Setup();

			Fog = new Fog(new Vector3(0.9f, 0.6f, 0.7f), 2, 0.1f);
			SkyColour = new Vector3(0.9f, 0.6f, 0.7f);

			Gravity = 0;
			
			WorldLight = new Light(
				new Vector3(0.1f, 0.1f, 0.1f),
				new Vector3(0),
				new Vector3(0),
				new Vector3(0, -1, 0)
			);

			FarPlane = 400f;
			CameraPosition = new Vector3(0, 40, 0);
			
			SetAlarm(new RealTimeAlarm(() => { Console.WriteLine("test"); }, 10));

			// type = Cube.AddType(new HeightBasedCubeType(new Vector3(0.79f, 0.8f, 0.83f), 0.1f, 1f, 32, 1, 1));

			AddComponent(new Player(_camera) { Position = new Vector3(0, 40, 0) });
		}

		public override void Build() {
			base.Build();

			Random rand = new Random();

			/*for (int i = 0; i < 32; i++) {
				for (int ii = 0; ii < 32; ii++) {
					for (int iii = 0; iii < 32; iii++) {
						AddCube(type, i, ii, iii);
					}
				}
			}

			AddCube(type, -10, 10, -10);

			

			for (int i = 0; i < 30; i++) {
				int x = 0, y = 0, z = 0;

				int side = rand.Next(4);

				switch (side) {
					case 0:
						x = -5;
						y = rand.Next(-5, 35);
						z = rand.Next(-5, 35);
						break;
					case 1:
						x = 35;
						y = rand.Next(-5, 35);
						z = rand.Next(-5, 35);
						break;
					case 2:
						z = -5;
						y = rand.Next(-5, 35);
						x = rand.Next(-5, 35);
						break;
					case 3:
						z = 35;
						y = rand.Next(-5, 35);
						x = rand.Next(-5, 35);
						break;
				}

				Vector3 colour = new Vector3(
					(float) rand.NextDouble(),
					(float) rand.NextDouble(),
					(float) rand.NextDouble()
				);

				AddLight(
					new PointLight(
						colour,
						new Vector3(x, y, z),
						rand.Next(5, 20)
					)
				);
			}*/

			for (int x = -256; x < 256; x++) {
				for (int z = -256; z < 256; z++) {
					int y = 16 + (int) Math.Round(8 * Math.Sin(x / (Math.PI * 4)) + 6 * Math.Cos(z / (Math.PI * 4)));

					for (int h = y; h >= 0; h--) {
						AddCube(type, x, h, z);
					}
				}
			}

			/*for (int i = 0; i < 100; i++) {
				AddLight(
					new PointLight(
						new Vector3(
						//1
						(float) rand.NextDouble(),
						(float) rand.NextDouble(),
						(float) rand.NextDouble()
						),
						new Vector3(
							rand.Next(-256, 256),
							rand.Next(10, 40),
							rand.Next(-256, 256)
						),
						rand.Next(30, 60)
					)
				);
			}*/

			for (int i = 0; i < 100; i++) {
				PlaceStructure(
					_rod,
					rand.Next(-200, 200),
					rand.Next(-200, 200)
				);
			}
		}

		public override void Update(FrameEventArgs args, MouseState mouse, KeyboardState state) {
			base.Update(args, mouse, state);
		}
	}
}
