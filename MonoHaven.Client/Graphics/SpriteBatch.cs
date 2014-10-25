using System;
using System.Collections.Generic;
using MonoHaven.Graphics.Shaders;
using OpenTK.Graphics.OpenGL;

namespace MonoHaven.Graphics
{
	public class SpriteBatch : IDisposable
	{
		private const string Coord2DShaderParam = "coord2d";
		private const string TexCoord = "texcoord";

		private Texture currentTexture;
		private bool isActive;

		private readonly Shader shader;

		private readonly List<float> vertices;
		private readonly VertexBuffer vertexBuffer;

		public SpriteBatch()
		{
			vertices = new List<float>();
			vertexBuffer = new VertexBuffer(BufferUsageHint.StreamDraw);

			var vertexShader = new VertexShaderTemplate();
			vertexShader.Session = new Dictionary<string, object>();
			vertexShader.Session["Coord2d"] = Coord2DShaderParam;
			vertexShader.Session["TexCoord"] = TexCoord;
			vertexShader.Initialize();

			var fragmentShader = new FragmentShaderTemplate();

			shader = new Shader(vertexShader.TransformText(), fragmentShader.TransformText());
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
			vertexBuffer.Fill(vertices.ToArray());
			Render();
			vertices.Clear();
		}

		public void Draw(Texture texture, IEnumerable<Vertex> vertices)
		{
			ChangeTexture(texture);

			foreach (var vertex in vertices)
			{
				AddVertex(vertex.X, vertex.Y, vertex.U, vertex.V);
			}
		}

		private void AddVertex(int x, int y, float u, float v)
		{
			vertices.Add(x);
			vertices.Add(y);
			vertices.Add(u);
			vertices.Add(v);
		}

		private void ChangeTexture(Texture texture)
		{
			if (currentTexture != texture)
			{
				Flush();
				currentTexture = texture;
			}
		}

		private void Render()
		{
			int coords = shader.GetAttributeLocation(Coord2DShaderParam);
			int texCoords = shader.GetAttributeLocation(TexCoord);

			GL.EnableVertexAttribArray(coords);
			GL.EnableVertexAttribArray(texCoords);

			GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBuffer.Id);
			GL.VertexAttribPointer(coords, 2, VertexAttribPointerType.Float, false, 4 * sizeof(float), 0);
			GL.VertexAttribPointer(texCoords, 2, VertexAttribPointerType.Float, false, 4 * sizeof(float), 2 * sizeof(float));

			if (currentTexture != null)
				GL.BindTexture(currentTexture.Target, currentTexture.Id);
			else
				GL.BindTexture(TextureTarget.Texture2D, 0);
			
			GL.DrawArrays(PrimitiveType.Quads, 0, vertices.Count / 4);
			
			GL.DisableVertexAttribArray(coords);
			GL.DisableVertexAttribArray(texCoords);
		}

		public void Dispose()
		{
			vertexBuffer.Dispose();
			shader.Dispose();
		}
	}
}
