using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
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

		public void Translate(Point p)
		{
			Translate(p.X, p.Y);
		}

		public void Draw(Drawable drawable, Point p)
		{
			Draw(drawable, p.X, p.Y);
		}

		public void Draw(Drawable drawable, int x, int y)
		{
			Draw(drawable, x, y, drawable.Width, drawable.Height);
		}

		public void Draw(Drawable drawable, int x, int y, int width, int height)
		{
			var vs = drawable.GetVertices(new Rectangle(x, y, width, height));
			spriteBatch.Draw(drawable.GetTexture(), vs.Select(TranslateVertex));
		}

		private Vertex TranslateVertex(Vertex v)
		{
			return new Vertex(v.X + offset.X, v.Y + offset.Y, v.U, v.V);
		}
	}
}

