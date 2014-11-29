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

		public Picture(TextureSlice tex, BitArray hitmask)
			: this(-1, tex, Point.Empty, 0, 0, hitmask)
		{
		}

		public Picture(
			int id,
			TextureSlice tex,
			Point offset,
			int z,
			int subz,
			BitArray hitmask)
		{
			if (hitmask != null && hitmask.Count != tex.Width * tex.Height)
				throw new ArgumentException("Invalid hitmask size");

			this.id = id;
			this.tex = tex;
			this.size = new Size(tex.Width, tex.Height);
			this.bounds = new Rectangle(offset.X, offset.Y, tex.Width, tex.Height);
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

		public override bool CheckHit(int x, int y)
		{
			if (hitmask != null)
			{
				if (bounds.Contains(x, y))
					return hitmask[(x - Offset.X) * Height + y - Offset.Y];
				return false;
			}
			return base.CheckHit(x, y);
			
		}
	}
}
