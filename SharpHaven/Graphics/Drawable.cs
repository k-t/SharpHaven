using System;

namespace SharpHaven.Graphics
{
	public abstract class Drawable : IDisposable
	{
		protected Coord2d size;

		public int Width
		{
			get { return size.X; }
		}

		public int Height
		{
			get { return size.Y; }
		}

		public Coord2d Size
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
