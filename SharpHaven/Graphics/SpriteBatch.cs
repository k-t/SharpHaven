using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;
using SharpHaven.Graphics.Shaders;

namespace SharpHaven.Graphics
{
	public class SpriteBatch : IDisposable
	{
		private const string Coord2DAttrName = "coord2d";
		private const string TexCoordAttrName = "texcoord";
		private const string ColorAttrName = "color";

		private readonly Texture empty;
		private readonly Shader shader;
		private readonly int coordAtrrib;
		private readonly int colorAttrib;
		private readonly int texCoordAttrib;

		private float color;
		private Texture currentTexture;
		private bool isActive;
		private int idx;
		private readonly float[] vertices;
		private readonly VertexBuffer vbo;

		public SpriteBatch()
		{
			vertices = new float[4 * 8 * 2000];
			vbo = new VertexBuffer(BufferUsageHint.StreamDraw);

			var vertexShader = new VertexShaderTemplate();
			vertexShader.Session = new Dictionary<string, object>();
			vertexShader.Session["Coord2d"] = Coord2DAttrName;
			vertexShader.Session["TexCoord"] = TexCoordAttrName;
			vertexShader.Session["Color"] = ColorAttrName;
			vertexShader.Initialize();

			var fragmentShader = new FragmentShaderTemplate();

			shader = new Shader(vertexShader.TransformText(), fragmentShader.TransformText());

			coordAtrrib = shader.GetAttributeLocation(Coord2DAttrName);
			colorAttrib = shader.GetAttributeLocation(ColorAttrName);
			texCoordAttrib = shader.GetAttributeLocation(TexCoordAttrName);
			
			empty = new Texture(1, 1);
			empty.Update(PixelFormat.Rgba, new byte[] {255, 255, 255, 255});

			SetColor(Color.White);
		}

#if DEBUG
		public int RenderCount;
#endif

		public void Dispose()
		{
			if (isActive) End();
			vbo.Dispose();
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
			Render();
			idx = 0;
		}

		public void SetColor(Color c)
		{
			var bytes = new[] {c.R, c.G, c.B, c.A};
			color = BitConverter.ToSingle(bytes, 0);
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
			vertices[idx++] = u;
			vertices[idx++] = v;
			vertices[idx++] = color;
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
#if DEBUG
			RenderCount++;
#endif

			GL.EnableVertexAttribArray(coordAtrrib);
			GL.EnableVertexAttribArray(colorAttrib);
			GL.EnableVertexAttribArray(texCoordAttrib);

			int stride = 5 * sizeof(float);

			GL.BindBuffer(BufferTarget.ArrayBuffer, vbo.Id);
			vbo.Fill(vertices, idx);

			int offset = 0;
			GL.VertexAttribPointer(coordAtrrib, 2, VertexAttribPointerType.Float, false, stride, offset);
			offset += 2;
			GL.VertexAttribPointer(texCoordAttrib, 2, VertexAttribPointerType.Float, false, stride, offset * sizeof(float));
			offset += 2;
			GL.VertexAttribPointer(colorAttrib, 4, VertexAttribPointerType.UnsignedByte, true, stride, offset * sizeof(float));
			
			if (currentTexture != null)
				GL.BindTexture(currentTexture.Target, currentTexture.Id);
			else
				GL.BindTexture(TextureTarget.Texture2D, 0);

			// TODO: Use triangles instead of quads?
			GL.DrawArrays(PrimitiveType.Quads, 0, idx / 5);

			GL.DisableVertexAttribArray(coordAtrrib);
			GL.DisableVertexAttribArray(colorAttrib);
			GL.DisableVertexAttribArray(texCoordAttrib);
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
		}
	}
}
