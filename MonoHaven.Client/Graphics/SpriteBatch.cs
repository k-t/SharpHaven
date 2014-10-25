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

		private readonly List<int> vertices;
		private readonly VertexBuffer vertexBuffer;

		private readonly List<float> texCoords;
		private readonly VertexBuffer texCoordBuffer;

		public SpriteBatch()
		{
			vertices = new List<int>();
			vertexBuffer = new VertexBuffer(BufferUsageHint.StreamDraw);
			texCoords = new List<float>();
			texCoordBuffer = new VertexBuffer(BufferUsageHint.StreamDraw);

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
			texCoordBuffer.Fill(texCoords.ToArray());

			Render();

			vertices.Clear();
			texCoords.Clear();
		}

		public void Draw(Texture texture, IEnumerable<Vertex> vertices)
		{
			ChangeTexture(texture);

			foreach (var vertex in vertices)
			{
				AddVertex(vertex.X, vertex.Y);
				AddTexCoord(vertex.U, vertex.V);
			}
		}

		private void AddVertex(int x, int y)
		{
			vertices.Add(x);
			vertices.Add(y);
		}

		private void AddTexCoord(float u, float v)
		{
			texCoords.Add(u);
			texCoords.Add(v);
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
			GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBuffer.Id);
			GL.EnableVertexAttribArray(coords);
			GL.VertexAttribPointer(coords, 2, VertexAttribPointerType.Int, false, 0, 0);

			int texCoords = shader.GetAttributeLocation(TexCoord);
			GL.BindBuffer(BufferTarget.ArrayBuffer, texCoordBuffer.Id);
			GL.EnableVertexAttribArray(texCoords);
			GL.VertexAttribPointer(texCoords, 2, VertexAttribPointerType.Float, false, 0, 0);

			if (currentTexture != null)
				GL.BindTexture(currentTexture.Target, currentTexture.Id);
			else
				GL.BindTexture(TextureTarget.Texture2D, 0);
			
			GL.DrawArrays(PrimitiveType.Quads, 0, vertices.Count / 2);
			
			GL.DisableVertexAttribArray(coords);
			GL.DisableVertexAttribArray(texCoords);
		}

		public void Dispose()
		{
			vertexBuffer.Dispose();
			texCoordBuffer.Dispose();
			shader.Dispose();
		}
	}
}
