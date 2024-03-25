using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using StbImageSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voxel.Components {
	internal class Texture {
		private int _id;
		public int ID { 
			get { return _id; } 
			protected set {
				if (_id == value) { return; }
				
				if (_loaded) {
					GL.DeleteTexture(_id);
				}

				_id = value;
			}
		}

		private Vector2 _size;
		public Vector2 Size { 
			get { return _size; } 
		}

		public int Width { get { return (int) _size.X; } }
		public int Height { get { return (int) _size.Y; } }

		private bool _loaded = false;

		public void Load (byte[] data, int width, int height, bool pixelated = true) {
			int id = GL.GenTexture();

			GL.BindTexture(TextureTarget.Texture2D, id);
			GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, width, height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, data);

			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter,
				pixelated ? (int) TextureMinFilter.Nearest : (int) TextureMinFilter.Linear);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter,
				pixelated ? (int) TextureMagFilter.Nearest : (int) TextureMagFilter.Linear);

			GL.BindTexture(TextureTarget.Texture2D, 0);

			_id = id;
			_size = new Vector2(width, height);
			_loaded = true;
		}

		public void Load (string path, bool pixelated = true) {
			path = "Content/" + path;
			if (!File.Exists(path)) {
				throw new Exception("File does not exist at \'" + path + "\'");
			}

			ImageResult image = ImageResult.FromStream(File.OpenRead(path), ColorComponents.RedGreenBlueAlpha);

			int id = GL.GenTexture();

			GL.BindTexture(TextureTarget.Texture2D, id);
			GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, image.Width, image.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, image.Data);

			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter,
				pixelated ? (int) TextureMinFilter.Nearest : (int) TextureMinFilter.Linear);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter,
				pixelated ? (int) TextureMagFilter.Nearest : (int) TextureMagFilter.Linear);

			GL.BindTexture(TextureTarget.Texture2D, 0);

			_id = id;
			_size = new Vector2(image.Width, image.Height);
			_loaded = true;
		}

		public void Use(TextureUnit unit) {
			if (!_loaded) { return; }
			GL.ActiveTexture(unit);
			GL.BindTexture(TextureTarget.Texture2D, ID);
		}

	}
}
