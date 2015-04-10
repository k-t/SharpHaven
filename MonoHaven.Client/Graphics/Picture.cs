using System;
using System.Collections;
using System.Drawing;

namespace MonoHaven.Graphics
{
	public class Picture : Drawable
	{
		private readonly TextureSlice tex;
		private readonly BitArray hitmask;

		public Picture(TextureSlice tex, BitArray hitmask)
		{
			if (hitmask != null && hitmask.Count != tex.Width * tex.Height)
				throw new ArgumentException("Invalid hitmask size");

			this.tex = tex;
			this.hitmask = hitmask;
			this.size = new Size(tex.Width, tex.Height);
		}

		public override void Draw(SpriteBatch batch, int x, int y, int w, int h)
		{
			tex.Draw(batch, x, y, w, h);
		}

		public override bool CheckHit(int x, int y)
		{
			if (hitmask != null)
			{
				if (x >= 0 && x < Width && y >= 0 && y < Height)
					return hitmask[x * Height + y];
				return false;
			}
			return base.CheckHit(x, y);
		}
	}
}
