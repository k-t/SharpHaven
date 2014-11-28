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

		public abstract void Draw(SpriteBatch batch, int x, int y, int w, int h);
	}
}
