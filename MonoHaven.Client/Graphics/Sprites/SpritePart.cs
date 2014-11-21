using System.Drawing;
using OpenTK.Graphics.OpenGL;

namespace MonoHaven.Graphics.Sprites
{
	public class SpritePart
	{
		private readonly int id;
		private readonly TextureSlice tex;
		private readonly Point offset;
		private readonly Size size;
		private readonly int z;
		private readonly int subz;

		public SpritePart(int id, TextureSlice tex, Point offset, Size size, int z, int subz)
		{
			this.id = id;
			this.tex = tex;
			this.offset = offset;
			this.size = size;
			this.z = z;
			this.subz = subz;
		}

		public int Id
		{
			get { return id; }
		}

		public TextureSlice Tex
		{
			get { return tex; }
		}

		public Point Offset
		{
			get { return offset; }
		}

		public Size Size
		{
			get { return size; }
		}

		public int Width
		{
			get { return size.Width; }
		}

		public int Height
		{
			get { return size.Height; }
		}

		public int Z
		{
			get { return z; }
		}

		public int SubZ
		{
			get { return subz; }
		}

		public void Draw(SpriteBatch batch, int x, int y)
		{
			Tex.Draw(batch,
				x + Offset.X,
				y + Offset.Y,
				Width,
				Height);
		}

		public bool CheckHit(int x, int y)
		{
			return x >= offset.X && x <= Width && y >= offset.Y && y <= Height;
		}
	}
}
