using System;
using System.Drawing;
using OpenTK.Graphics.OpenGL;

namespace MonoHaven
{
	public class GOut
	{
		public GOut()
		{
		}

		public void Draw(Tex tex, int x, int y)
		{
			tex.Render(x, y, tex.Width, tex.Height);
		}

		public void PushMatrix()
		{
			GL.PushMatrix();
		}

		public void PopMatrix()
		{
			GL.PopMatrix();
		}

		public void Translate(Point p)
		{
			GL.Translate(p.X, p.Y, 0);
		}
	}
}

