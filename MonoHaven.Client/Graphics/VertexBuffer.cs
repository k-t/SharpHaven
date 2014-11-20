using System;
using OpenTK.Graphics.OpenGL;

namespace MonoHaven.Graphics
{
	public class VertexBuffer : IDisposable
	{
		private int id;
		private BufferUsageHint usageHint;

		public VertexBuffer(BufferUsageHint usageHint)
		{
			GL.GenBuffers(1, out id);
			this.usageHint = usageHint;
		}

		public int Id
		{
			get { return id; }
		}

		public void Fill(int[] data)
		{
			GL.BindBuffer(BufferTarget.ArrayBuffer, id);
			GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(sizeof(int) * data.Length), data, usageHint);
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
		}

		public void Fill(float[] data, int count)
		{
			GL.BindBuffer(BufferTarget.ArrayBuffer, id);
			GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(sizeof(float) * count), data, usageHint);
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
		}

		public void Dispose()
		{
			GL.DeleteBuffers(1, ref id);
			id = 0;
		}
	}
}
