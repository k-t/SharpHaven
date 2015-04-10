using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using SharpHaven.Graphics.Sprites;

namespace SharpHaven.Graphics
{
	public class DrawingContext : IDisposable
	{
		private readonly INativeWindow window;
		private readonly SpriteBatch spriteBatch;
		private Point offset;
		private readonly Stack<Point> offsetStack;

		public DrawingContext(INativeWindow window, SpriteBatch spriteBatch)
		{
			this.window = window;
			this.offset = Point.Empty;
			this.offsetStack = new Stack<Point>();
			this.spriteBatch = spriteBatch;
			this.spriteBatch.Begin();
		}

		public void Dispose()
		{
			spriteBatch.End();
		}

		public void SetClip(int x, int y, int width, int height)
		{
			// flush unclipped drawables
			spriteBatch.Flush();
			// window coordinates have the origin at the bottom-left
			GL.Scissor(x + offset.X, window.Height - offset.Y - y - height, width, height);
		}

		public void ResetClip()
		{
			// flush clipped drawables
			spriteBatch.Flush();
			GL.Scissor(0, 0, window.Width, window.Height);
		}

		public void SetColor(byte r, byte g, byte b, byte a)
		{
			spriteBatch.SetColor(Color.FromArgb(a, r, g, b));
		}

		public void SetColor(Color color)
		{
			spriteBatch.SetColor(color);
		}

		public void ResetColor()
		{
			spriteBatch.SetColor(Color.White);
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
			drawable.Draw(spriteBatch, x + offset.X, y + offset.Y, width, height);
		}

		public void DrawRectangle(int x, int y, int width, int height)
		{
			spriteBatch.Draw(x + offset.X, y + offset.Y, width, height);
		}

		public void Draw(ISprite sprite, int x, int y)
		{
			foreach (var part in sprite.Parts.OrderBy(s => s.Z))
				Draw(part, x, y);
		}

		public void Draw(ISprite sprite, Point p)
		{
			Draw(sprite, p.X, p.Y);
		}

		public void Draw(SpritePart part, int x, int y)
		{
			Draw(part.Image, x + part.Offset.X, y + part.Offset.Y, part.Width, part.Height);
		}
	}
}

