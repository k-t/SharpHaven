using System;
using System.Drawing;
using MonoHaven.Graphics;
using OpenTK.Input;

namespace MonoHaven.UI.Widgets
{
	public abstract class Widget : IDisposable
	{
		private bool disposed;
		private Rectangle bounds;

		protected Widget()
		{
			Visible = true;
		}

		public int X
		{
			get { return bounds.X; }
			set { bounds.X = value; }
		}

		public int Y
		{
			get { return bounds.Y; }
			set { bounds.Y = value; }
		}

		public int Width
		{
			get { return bounds.Width; }
			set { bounds.Width = value; }
		}

		public int Height
		{
			get { return bounds.Height; }
			set { bounds.Height = value; }
		}

		public Rectangle Bounds
		{
			get { return bounds; }
			set { bounds = value; }
		}

		public bool Visible { get; set; }

		public void Dispose()
		{
			if (this.disposed)
				return;

			CleanUp();
			this.disposed = true;
		}

		protected virtual void CleanUp() {}

		public virtual void Draw(DrawingContext g) { }
		public virtual void OnButtonDown(MouseButtonEventArgs e) {}
		public virtual void OnButtonUp(MouseButtonEventArgs e) {}
		public virtual void OnKeyDown(KeyboardKeyEventArgs e) { }
		public virtual void OnKeyUp(KeyboardKeyEventArgs e) {}
		public virtual void OnMouseMove(MouseMoveEventArgs e) {}
	}
}

