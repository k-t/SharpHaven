using System;
using System.Collections.Generic;
using System.Drawing;
using OpenTK.Graphics.OpenGL;

namespace MonoHaven.Graphics
{
	public class DrawingContext : IDisposable
	{
		private readonly SpriteBatch spriteBatch;
		private Point offset;
		private readonly Stack<Point> offsetStack;

		public DrawingContext(SpriteBatch spriteBatch)
		{
			this.offset = Point.Empty;
			this.offsetStack = new Stack<Point>();
			this.spriteBatch = spriteBatch;
			this.spriteBatch.Begin();
		}

		public void Dispose()
		{
			spriteBatch.End();
		}

		public void PushMatrix()
		{
			offsetStack.Push(offset);
		}

		public void PopMatrix()
		{
			offset = offsetStack.Pop();
		}

		public void Translate(int x, int y)
		{
			offset = Point.Add(offset, new Size(x, y));
		}

		public void Translate(Point offset)
		{
			Translate(offset.X, offset.Y);
		}

		public void Draw(Text text, Point p)
		{
			Draw(text, p.X, p.Y);
		}

		public void Draw(Text text, int x, int y)
		{
			Draw(text.Texture, x, y);
		}

		public void Draw(Texture texture, Point p)
		{
			Draw(texture, p.X, p.Y);
		}

		public void Draw(Texture texture, int x, int y)
		{
			spriteBatch.Draw(texture, x + offset.X, y + offset.Y);
		}

		public void Draw(TextureRegion region, int x, int y)
		{
			spriteBatch.Draw(region, x + offset.X, y + offset.Y);
		}
	}
}

