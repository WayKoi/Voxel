﻿using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voxel.Structs;

namespace Voxel.Components {
	internal class Chunk {
		private Vector3 _position;

		private List<Vertex3D> points = new List<Vertex3D>();
		private Cube?[,,] cubes;
		private int _chunksize;
		private float _chunkScale = 1f;

		private int VAO, VBO;

		public Chunk(int x, int y, int z, int chunkSize = 32) {
			_position = new Vector3(x, y, z);

			cubes = new Cube?[chunkSize, chunkSize, chunkSize];

			_chunksize = chunkSize;
		}

		public void Load () {
			VBO = Vertex3D.GenVBO(new Vertex3D[0]);
			VAO = Vertex3D.GenVAO(VBO);
		}

		public void Add (Cube cube, Vector3 Position) {
			cubes[(int) Position.X, (int) Position.Y, (int) Position.Z] = cube;

			Update();
		}

		public void Add(Cube[] adds, Vector3[] Positions) {
			for (int i = 0; i < adds.Length && i < Positions.Length; i++) {
				cubes[(int) Positions[i].X, (int) Positions[i].Y, (int) Positions[i].Z] = adds[i];
			}

			Update();
		}

		private void Update () {
			points.Clear();

			int[,,] draw = new int[_chunksize, _chunksize, 6];
			bool[,,] plane = new bool[_chunksize, _chunksize, 6];
			bool[,,] obscure = new bool[_chunksize, _chunksize, 6];

			for (int a = 0; a < _chunksize; a++) {
				for (int b = 0; b < _chunksize; b++) {
					for (int c = 0; c < _chunksize; c++) {
						/*
							Front = 0,
							Back = 1,
							Left = 2,
							Right = 3,
							Top = 4,
							Bottom = 5
						 */

						int[,] sends = {
							{ b, c, _chunksize - 1 - a },
							{ b, c, a },
							{ a, b, c },
							{ _chunksize - 1 - a, b, c },
							{ b, _chunksize - 1 - a, c },
							{ b, a, c }
						};

						for (int i = 0; i < 6; i++) {
							int x = sends[i, 0], y = sends[i, 1], z = sends[i, 2];

							plane[b, c, i] = CubeExists(x, y, z);
							if (!FaceObscured(x, y, z, obscure, i)) {
								// add to the drawing plane
								draw[b, c, i] = 1;					
							}

						}
					}
				}

				// optimize the drawing plane
				// add the points to the draw

				for (int b = 0; b < _chunksize; b++) {
					for (int c = 0; c < _chunksize; c++) {
						int[,] sends = {
							{ b, c, _chunksize - 1 - a },
							{ b, c, a },
							{ a, b, c },
							{ _chunksize - 1 - a, b, c },
							{ b, _chunksize - 1 - a, c },
							{ b, a, c }
						};
						
						for (int i = 0; i < 6; i++) {
							if (draw[b, c, i] > 0) {
								
							}
						}
						
					}
				}

				

                obscure = plane;
				plane = new bool[_chunksize, _chunksize, 6];
			}

			Vertex3D.BufferVertices(VBO, points.ToArray(), BufferUsageHint.StaticDraw);
		}

		public Vector2 MakeBlock(int x, int y, int[,,] draw, int index) {
			int width = 1, height = 1;
			


			return new Vector2(width, height);
		}

		public bool FaceObscured(int x, int y, int z, bool[,,] obscure, int plane) {
			return cubes[x, y, z] != null && obscure[y, z, plane];      
		}

		public bool CubeExists(int x, int y, int z) {
			return cubes[x, y, z] != null;
		}

		public void Render () {
			GL.BindVertexArray(VAO);
			GL.DrawArrays(PrimitiveType.Triangles, 0, points.Count);
			GL.BindVertexArray(0);
		}

	}
}
