using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using MonoHaven.Graphics;
using MonoHaven.Utils;
using OpenTK.Input;

namespace MonoHaven.UI
{
	public abstract class Widget : IDisposable
	{
		private Widget parent;
		private Widget next;
		private Widget previous;
		private Widget firstChild;
		private Widget lastChild;
		private Rectangle bounds;
		private bool isDisposed;

		protected Widget()
		{
			Visible = true;
		}

		private IEnumerable<Widget> Children
		{
			get
			{
				for (var child = firstChild; child != null; child = child.next)
					yield return child;
			}
		}

		private IEnumerable<Widget> ReversedChildren
		{
			get
			{
				for (var child = lastChild; child != null; child = child.previous)
					yield return child;
			}
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

		public Point Location
		{
			get { return bounds.Location; }
			set { bounds.Location = value; }
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

		public Size Size
		{
			get { return bounds.Size; }
			set { bounds.Size = value; }
		}

		public Rectangle Bounds
		{
			get { return bounds; }
			set { bounds = value; }
		}

		public bool Visible { get; set; }

		public void Add(Widget widget)
		{
			if (widget.parent != null)
				widget.parent.Remove(widget);

			widget.parent = this;

			if (this.firstChild == null)
				this.firstChild = widget;

			if (this.lastChild != null)
			{
				widget.previous = this.lastChild;
				this.lastChild.next = widget;
				this.lastChild = widget;
			}
			else
			{
				this.lastChild = widget;
			}
		}

		public void Remove(Widget widget)
		{
			if (this.firstChild == widget)
				this.firstChild = widget.next;
			if (this.lastChild == widget)
				this.lastChild = widget.previous;

			if (widget.next != null)
				widget.next.previous = widget.previous;
			if (widget.previous != null)
				widget.previous.next = widget.next;

			widget.parent = null;
		}

		public void Dispose()
		{
			if (isDisposed)
				return;

			OnDispose();
			isDisposed = true;
		}

		public void Draw(DrawingContext g)
		{
			g.PushMatrix();
			g.Translate(X, Y);
			// draw itself
			OnDraw(g);
			// draw all children
			foreach (var widget in Children.Where(x => x.Visible))
				widget.Draw(g);
			g.PopMatrix();
		}

		public Widget GetChildAt(Point p)
		{
			p = p.Sub(Location);
			foreach (var widget in ReversedChildren)
			{
				if (widget.Bounds.Contains(p.X, p.Y))
					return widget.GetChildAt(p) ?? widget;
			}
			return null;
		}

		public virtual void OnButtonDown(MouseButtonEventArgs e) {}
		public virtual void OnButtonUp(MouseButtonEventArgs e) {}
		public virtual void OnKeyDown(KeyboardKeyEventArgs e) { }
		public virtual void OnKeyUp(KeyboardKeyEventArgs e) {}
		public virtual void OnMouseMove(MouseMoveEventArgs e) {}

		protected virtual void OnDraw(DrawingContext g) {}
		protected virtual void OnDispose() {}
	}
}

