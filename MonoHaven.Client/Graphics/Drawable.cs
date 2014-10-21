using System;
using System.Collections.Generic;
using System.Drawing;

namespace MonoHaven.Graphics
{
	public abstract class Drawable : IDisposable
	{
		public int Width { get; set; }
		public int Height { get; set; }

		public abstract Texture GetTexture();
		public abstract IEnumerable<Vertex> GetVertices(Rectangle region);

		public virtual void Dispose() {}
	}
}
