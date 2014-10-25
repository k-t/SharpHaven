using System;

namespace MonoHaven.Graphics
{
	public abstract class Drawable : IDisposable
	{
		public int Width { get; set; }
		public int Height { get; set; }

		public abstract void Draw(SpriteBatch batch, int x, int y, int w, int h);

		public virtual void Dispose() {}
	}
}
