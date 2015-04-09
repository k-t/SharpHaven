using System;
using System.Collections;
using System.Drawing;

namespace MonoHaven.Graphics
{
	public class Picture : Drawable
	{
		private readonly int id;
		private readonly TextureSlice tex;
		private Rectangle bounds;
		private readonly int z;
		private readonly int subz;
		private readonly BitArray hitmask;
		private readonly Drawable innerDrawable;

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
			this.bounds = new Rectangle(offset.X, offset.Y, tex.Width, tex.Height);
			this.size = bounds.Size;
			this.z = z;
			this.subz = subz;
			this.hitmask = hitmask;
		}

		// TODO: it's better to create a new class that will wrap both Picture and TextLine
		public Picture(
			Drawable drawable,
			Point offset,
			int z,
			int subz)
		{
			this.innerDrawable = drawable;
			this.bounds = new Rectangle(offset.X, offset.Y, drawable.Width, drawable.Height);
			this.size = bounds.Size;
			this.z = z;
			this.subz = subz;
		}

		public int Id
		{
			get { return id; }
		}

		public Point Offset
		{
			get { return bounds.Location; }
			set { bounds.Location = value; }
		}

		public int Z
		{
			get { return z; }
		}

		public int SubZ
		{
			get { return subz; }
		}

		public TextureSlice Tex
		{
			get { return tex; }
		}

		public BitArray Hitmask
		{
			get { return hitmask; }
		}

		public override void Draw(SpriteBatch batch, int x, int y, int w, int h)
		{
			if (innerDrawable != null)
				innerDrawable.Draw(batch, x + Offset.X, y + Offset.Y, w, h);
			else
				tex.Draw(batch, x + Offset.X, y + Offset.Y, w, h);
		}

		public override bool CheckHit(int x, int y)
		{
			if (innerDrawable != null)
				return innerDrawable.CheckHit(x, y);

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
