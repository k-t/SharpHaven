using System;

namespace MonoHaven.Graphics
{
	public abstract class Drawable : IDisposable
	{
		protected int width;
		protected int height;

		public int Width
		{
			get { return width; }
		}

		public int Height
		{
			get { return height; }
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
