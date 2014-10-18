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

		public void DrawImage(Point p, Texture texture)
		{
			DrawImage(p.X, p.Y, texture);
		}

		public void DrawImage(int x, int y, Texture texture)
		{
			spriteBatch.Draw(texture, x + offset.X, y + offset.Y);
		}

		public void DrawImage(int x, int y, TextureRegion region)
		{
			spriteBatch.Draw(region, x + offset.X, y + offset.Y);
		}
	}
}

