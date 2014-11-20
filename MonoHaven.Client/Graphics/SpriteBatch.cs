using System;
using System.Collections.Generic;
using System.Drawing;
using MonoHaven.Graphics.Shaders;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace MonoHaven.Graphics
{
	public class SpriteBatch : IDisposable
	{
		private const string Coord2DAttrName = "coord2d";
		private const string TexCoordAttrName = "texcoord";
		private const string ColorAttrName = "color";

		private Color4 color = Color4.White;
		private Texture currentTexture;
		private Texture empty;
		private bool isActive;

		private readonly Shader shader;

		private int idx;
		private readonly float[] vertices;
		private readonly VertexBuffer vertexBuffer;

		public SpriteBatch()
		{
			vertices = new float[4 * 8 * 2000];
			vertexBuffer = new VertexBuffer(BufferUsageHint.StreamDraw);

			var vertexShader = new VertexShaderTemplate();
			vertexShader.Session = new Dictionary<string, object>();
			vertexShader.Session["Coord2d"] = Coord2DAttrName;
			vertexShader.Session["TexCoord"] = TexCoordAttrName;
			vertexShader.Session["Color"] = ColorAttrName;
			vertexShader.Initialize();

			var fragmentShader = new FragmentShaderTemplate();

			shader = new Shader(vertexShader.TransformText(), fragmentShader.TransformText());
			
			empty = new Texture(1, 1);
			empty.Update(PixelFormat.Rgba, new byte[] {255, 255, 255, 255});
		}

		public void Dispose()
		{
			if (isActive) End();
			vertexBuffer.Dispose();
			shader.Dispose();
		}

		public void Begin()
		{
			if (isActive) return;
			
			shader.Begin();
			isActive = true;
		}

		public void End()
		{
			if (!isActive) return;

			Flush();

			shader.End();
			isActive = false;
		}

		public void Flush()
		{
			vertexBuffer.Fill(vertices, idx);
			Render();
			idx = 0;
		}

		public void SetColor(Color color)
		{
			this.color = color;
		}

		/// <summary>
		/// Draws quad.
		/// </summary>
		public void Draw(int qx, int qy, int qw, int qh)
		{
			ChangeTexture(empty);
			AddVertex(qx, qy, 0.0f, 0.0f);
			AddVertex(qx + qw, qy, 0.0f, 0.0f);
			AddVertex(qx + qw, qy + qh, 0.0f, 0.0f);
			AddVertex(qx, qy + qh, 0.0f, 0.0f);
		}

		/// <summary>
		/// Draws textured quad.
		/// </summary>
		/// <param name="tex">Texture.</param>
		/// <param name="qx">Quad x-position.</param>
		/// <param name="qy">Quad y-position.</param>
		/// <param name="qw">Quad width.</param>
		/// <param name="qh">Quad height</param>
		/// <param name="u">Left texture coordinate.</param>
		/// <param name="v">Top texture coordinate</param>
		/// <param name="u2">Right texture coordinate.</param>
		/// <param name="v2">Bottom texture coordinate.</param>
		public void Draw(Texture tex, int qx, int qy, int qw, int qh,
			float u, float v, float u2, float v2)
		{
			ChangeTexture(tex);
			AddVertex(qx, qy, u, v);
			AddVertex(qx + qw, qy, u2, v);
			AddVertex(qx + qw, qy + qh, u2, v2);
			AddVertex(qx, qy + qh, u, v2);
		}

		private void AddVertex(int x, int y, float u, float v)
		{
			vertices[idx++] = x;
			vertices[idx++] = y;
			// TODO: pack colors
			vertices[idx++] = color.R;
			vertices[idx++] = color.G;
			vertices[idx++] = color.B;
			vertices[idx++] = color.A;
			vertices[idx++] = u;
			vertices[idx++] = v;
		}

		private void ChangeTexture(Texture texture)
		{
			if (currentTexture != texture)
			{
				Flush();
				currentTexture = texture;
			}
			else if (idx == vertices.Length)
			{
				Flush();
			}
		}

		private void Render()
		{
			int coords = shader.GetAttributeLocation(Coord2DAttrName);
			int color = shader.GetAttributeLocation(ColorAttrName);
			int texCoords = shader.GetAttributeLocation(TexCoordAttrName);

			GL.EnableVertexAttribArray(coords);
			GL.EnableVertexAttribArray(color);
			GL.EnableVertexAttribArray(texCoords);

			int stride = 8 * sizeof(float);

			GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBuffer.Id);
			GL.VertexAttribPointer(coords, 2, VertexAttribPointerType.Float, false, stride, 0);
			GL.VertexAttribPointer(color, 4, VertexAttribPointerType.Float, false, stride, 2 * sizeof(float));
			GL.VertexAttribPointer(texCoords, 2, VertexAttribPointerType.Float, false, stride, 6 * sizeof(float));

			if (currentTexture != null)
				GL.BindTexture(currentTexture.Target, currentTexture.Id);
			else
				GL.BindTexture(TextureTarget.Texture2D, 0);

			// TODO: Use triangles instead of quads?
			GL.DrawArrays(PrimitiveType.Quads, 0, idx / 8);
			
			GL.DisableVertexAttribArray(coords);
			GL.DisableVertexAttribArray(color);
			GL.DisableVertexAttribArray(texCoords);
		}
	}
}
