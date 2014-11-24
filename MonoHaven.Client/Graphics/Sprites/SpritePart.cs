using System;
using System.Collections;
using System.Drawing;

namespace MonoHaven.Graphics.Sprites
{
	public class SpritePart
	{
		private readonly int id;
		private readonly TextureSlice tex;
		private readonly Rectangle bounds;
		private readonly int z;
		private readonly int subz;
		private readonly BitArray hitmask;

		public SpritePart(
			int id,
			TextureSlice tex,
			Point offset,
			Size size,
			int z,
			int subz,
			BitArray hitmask)
		{
			if (hitmask.Count != tex.Width * tex.Height)
				throw new ArgumentException("Invalid hitmask size");

			this.id = id;
			this.tex = tex;
			this.bounds = new Rectangle(offset, size);
			this.z = z;
			this.subz = subz;
			this.hitmask = hitmask;
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
			get { return bounds.Location; }
		}

		public Size Size
		{
			get { return bounds.Size; }
		}

		public int Width
		{
			get { return bounds.Width; }
		}

		public int Height
		{
			get { return bounds.Height; }
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
			if (bounds.Contains(x, y))
				return hitmask[(x - Offset.X) * Height + y - Offset.Y];
			return false;
		}
	}
}
