using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace Voxel.Components {
	internal class World {

		private Dictionary<(int, int, int), Chunk> _chunks = new Dictionary<(int, int, int), Chunk>();
		private bool _initialized = false;

		public World() { }

		public void Init () {
			if (_initialized) { return; }

			foreach (KeyValuePair<(int, int, int), Chunk> pair in _chunks) {
				int[][] edges = new int[6][];
				(int x, int y, int z) chunkPos = pair.Key;

				// left chunk, right face
				if (_chunks.ContainsKey((chunkPos.x - 1, chunkPos.y, chunkPos.z))) {
					edges[(int) Face.Left] = _chunks[(chunkPos.x - 1, chunkPos.y, chunkPos.z)].GetEdge(Face.Right);
				}

				// right chunk, left face
				if (_chunks.ContainsKey((chunkPos.x + 1, chunkPos.y, chunkPos.z))) {
					edges[(int) Face.Right] = _chunks[(chunkPos.x + 1, chunkPos.y, chunkPos.z)].GetEdge(Face.Left);
				}

				// top chunk, bottom face
				if (_chunks.ContainsKey((chunkPos.x, chunkPos.y + 1, chunkPos.z))) {
					edges[(int) Face.Top] = _chunks[(chunkPos.x, chunkPos.y + 1, chunkPos.z)].GetEdge(Face.Bottom);
				}

				// bottom chunk, top face
				if (_chunks.ContainsKey((chunkPos.x, chunkPos.y - 1, chunkPos.z))) {
					edges[(int) Face.Bottom] = _chunks[(chunkPos.x, chunkPos.y - 1, chunkPos.z)].GetEdge(Face.Top);
				}

				pair.Value.Load(edges);
			}

			_initialized = true;
		}

		public void AddCube (int id, int x, int y, int z) {
			bool[] pos = new bool[] {
				x < 0,
				y < 0,
				z < 0
			};

			int cx = x / Chunk.Size + (pos[0] ? -1 : 0);
			int cy = y / Chunk.Size + (pos[1] ? -1 : 0);
			int cz = z / Chunk.Size + (pos[2] ? -1 : 0);

			if (!_chunks.ContainsKey((cx, cy, cz))) {
				_chunks.Add(
					(cx, cy, cz), 
					new Chunk(
						new Vector3(
							cx * Chunk.Size, 
							cy * Chunk.Size, 
							cz * Chunk.Size
						)
					)
				);
			}

			int px = pos[0] ? (Chunk.Size - 1) - (Math.Abs(x + 1) % Chunk.Size) : x % Chunk.Size;
			int py = pos[1] ? (Chunk.Size - 1) - (Math.Abs(y + 1) % Chunk.Size) : y % Chunk.Size;
			int pz = pos[2] ? (Chunk.Size - 1) - (Math.Abs(z + 1) % Chunk.Size) : z % Chunk.Size;

			_chunks[(cx, cy, cz)].AddCubes(id, px, py, pz);
		}

		public void Render () {
			foreach (KeyValuePair<(int, int, int), Chunk> pair in _chunks) {
				pair.Value.Render();
			}
		}

		/*private Dictionary<(int, int, int), Chunk> _chunks = new Dictionary<(int, int, int), Chunk>();

		private bool _loaded = false;

		public World() { }

		public void Load() {
			if (_loaded) { return; }
			_loaded = true;

			foreach (KeyValuePair<(int, int, int), Chunk> pair in _chunks) {
				pair.Value.Load();
			}
		}

		public void AddCube(int cx, int cy, int cz, int x, int y, int z, Cube cube, bool update = true) {
			if (!_chunks.ContainsKey((x, y, z))) {
				Chunk piece = new Chunk(x * 32 + (x < 0 ? 1 : 0), y * 32 + (y < 0 ? 1 : 0), z * 32 + (z < 0 ? 1 : 0));
				if (_loaded) { piece.Load(); }
				_chunks.Add((x, y, z), piece);
			}

			cx = cx < 0 ? 31 - (Math.Abs(cx) % 32) : cx % 32;
			cy = cy < 0 ? 31 - (Math.Abs(cy) % 32) : cy % 32;
			cz = cz < 0 ? 31 - (Math.Abs(cz) % 32) : cz % 32;

			Chunk chunk = _chunks[(x, y, z)];
			chunk.Add(cube, new Vector3i(cx, cy, cz));
			if (update) { chunk.Update(); }
		}

		public void AddCubes(List<Vector3i> positions, List<Cube> cubes) {
			List<(int, int, int)> adds = new List<(int, int, int)>();

			int poscount = positions.Count, cubecount = cubes.Count;
			for (int i = 0; i < poscount && i < cubecount; i++) {
				(int x, int y, int z) chunk = (
					positions[i].X / 32 + (positions[i].X < 0 ? -1 : 0),
					positions[i].Y / 32 + (positions[i].Y < 0 ? -1 : 0),
					positions[i].Z / 32 + (positions[i].Z < 0 ? -1 : 0)
				);

				if (!adds.Contains(chunk)) { adds.Add(chunk); }

				AddCube(positions[i].X, positions[i].Y, positions[i].Z, chunk.x, chunk.y, chunk.z, cubes[i], false);
			}

			foreach ((int x, int y, int z) chunk in adds) {
				bool[,,] obscure = new bool[32, 32, 6];

				_chunks[chunk].Update(obscure);
			}
		}

		public void Render() {
			foreach (KeyValuePair<(int, int, int), Chunk> pair in _chunks) {
				pair.Value.Render();
			}
		}*/

	}
}
