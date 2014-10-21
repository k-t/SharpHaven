using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;

namespace MonoHaven.Graphics
{
	public class SpriteBatch : IDisposable
	{
		private Texture currentTexture;
		private bool isActive;

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
		}

		public void Begin()
		{
			if (isActive) return;
			
			isActive = true;
		}

		public void End()
		{
			if (!isActive) return;

			Flush();
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
				AddVertex(vertex.X, vertex.Y, 0);
				AddTexCoord(vertex.U, vertex.V);
			}
		}

		private void AddVertex(int x, int y, int z)
		{
			vertices.Add(x);
			vertices.Add(y);
			vertices.Add(z);
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
			if (currentTexture == null)
				return;

			GL.EnableClientState(ArrayCap.VertexArray);
			GL.EnableClientState(ArrayCap.TextureCoordArray);
			
			GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBuffer.Id);
			GL.VertexPointer(3, VertexPointerType.Int, 0, 0);

			GL.BindBuffer(BufferTarget.ArrayBuffer, texCoordBuffer.Id);
			GL.TexCoordPointer(2, TexCoordPointerType.Float, 0, 0);

			GL.BindTexture(currentTexture.Target, currentTexture.Id);
			GL.DrawArrays(PrimitiveType.Quads, 0, vertices.Count / 3);

			GL.DisableClientState(ArrayCap.VertexArray);
			GL.DisableClientState(ArrayCap.TextureCoordArray);
		}

		public void Dispose()
		{
			vertexBuffer.Dispose();
			texCoordBuffer.Dispose();
		}
	}
}
