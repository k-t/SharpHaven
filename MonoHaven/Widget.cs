using System;
using System.Drawing;
using OpenTK.Input;

namespace MonoHaven
{
	public abstract class Widget : IDisposable
	{
		private bool disposed;
		private Rectangle bounds;

		public Widget(Size sz) : this(Point.Empty, sz)
		{}

		public Widget(Point p, Size sz) : this(new Rectangle(p, sz))
		{}

		public Widget(Rectangle bounds)
		{
			this.bounds = bounds;
			this.Visible = true;
		}

		public Rectangle Bounds
		{
			get { return bounds; }
		}

		public Point Location
		{
			get { return bounds.Location; }
		}

		public bool Visible { get; set; }

		public void Dispose()
		{
			if (this.disposed)
				return;

			CleanUp();
			this.disposed = true;
		}

		public virtual void Draw(GOut g)
		{}

		protected virtual void CleanUp()
		{}

		public virtual void OnButtonDown(MouseButtonEventArgs e)
		{}

		public virtual void OnButtonUp(MouseButtonEventArgs e)
		{}
	}
}

