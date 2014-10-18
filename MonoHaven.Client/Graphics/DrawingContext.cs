using System.Drawing;
using OpenTK.Graphics.OpenGL;

namespace MonoHaven.Graphics
{
	public class DrawingContext
	{
		public void PushMatrix()
		{
			GL.PushMatrix();
		}

		public void PopMatrix()
		{
			GL.PopMatrix();
		}

		public void Translate(int x, int y)
		{
			GL.Translate(x, y, 0);
		}

		public void Translate(Point p)
		{
			Translate(p.X, p.Y);
		}

		public void DrawImage(Point p, Texture texture)
		{
			DrawImage(p.X, p.Y, texture);
		}

		public void DrawImage(int x, int y, Texture texture)
		{
			texture.Bind();
			GL.Begin(BeginMode.Quads);
			{
				GL.TexCoord2(0, 0);
				GL.Vertex3(x, y, 0);

				GL.TexCoord2(1, 0);
				GL.Vertex3(x + texture.Width, y, 0);

				GL.TexCoord2(1, 1);
				GL.Vertex3(x + texture.Width, y + texture.Height, 0);

				GL.TexCoord2(0, 1);
				GL.Vertex3(x, y + texture.Height, 0);
			}
			GL.End();
		}

		public void DrawImage(int x, int y, TextureRegion region)
		{
			var texture = region.Texture;
			texture.Bind();
			GL.Begin(BeginMode.Quads);
			{
				GL.TexCoord2(region.Left, region.Top);
				GL.Vertex3(x, y, 0);

				GL.TexCoord2(region.Right, region.Top);
				GL.Vertex3(x + region.Width, y, 0);

				GL.TexCoord2(region.Right, region.Bottom);
				GL.Vertex3(x + region.Width, y + region.Height, 0);

				GL.TexCoord2(region.Left, region.Bottom);
				GL.Vertex3(x, y + region.Height, 0);
			}
			GL.End();
		}
	}
}

