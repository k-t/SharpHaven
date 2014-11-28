using System;
using System.Collections;
using System.Drawing;

namespace MonoHaven.Graphics
{
	public class Picture : Drawable
	{
		private readonly int id;
		private readonly TextureSlice tex;
		private readonly Rectangle bounds;
		private readonly int z;
		private readonly int subz;
		private readonly BitArray hitmask;

		public Picture(
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
			this.width = size.Width;
			this.height = size.Height;
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

		public int Z
		{
			get { return z; }
		}

		public int SubZ
		{
			get { return subz; }
		}

		public override void Draw(SpriteBatch batch, int x, int y, int w, int h)
		{
			Tex.Draw(batch, x + Offset.X, y + Offset.Y, w, h);
		}

		public bool CheckHit(int x, int y)
		{
			if (bounds.Contains(x, y))
				return hitmask[(x - Offset.X) * Height + y - Offset.Y];
			return false;
		}
	}
}
