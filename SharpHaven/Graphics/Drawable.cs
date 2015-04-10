using System;
using System.Drawing;

namespace SharpHaven.Graphics
{
	public abstract class Drawable : IDisposable
	{
		protected Size size;

		public int Width
		{
			get { return size.Width; }
		}

		public int Height
		{
			get { return size.Height; }
		}

		public Size Size
		{
			get { return size; }
		}

		public virtual void Dispose()
		{
		}

		public virtual bool CheckHit(int x, int y)
		{
			return x >= 0 && x < Width && y >= 0 && y < Height;
		}

		public abstract void Draw(SpriteBatch batch, int x, int y, int w, int h);
	}
}
