using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using MonoHaven.Graphics;
using MonoHaven.Utils;
using OpenTK;
using OpenTK.Input;

namespace MonoHaven.UI
{
	public abstract class Widget : IDisposable, IInputListener
	{
		private readonly IWidgetHost host;
		private Widget parent;
		private Widget next;
		private Widget previous;
		private Widget firstChild;
		private Widget lastChild;
		private Rectangle bounds;
		private bool isDisposed;
		private bool isFocused;
		private bool isHovered;

		protected Widget(IWidgetHost host)
		{
			this.host = host;
			Visible = true;
		}

		protected Widget(Widget parent)
			: this(parent.Host)
		{
			parent.AddChild(this);
		}

		#region Properties

		protected IWidgetHost Host
		{
			get { return host; }
		}

		protected Widget Parent
		{
			get { return parent; }
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
		}

		public int Width
		{
			get { return bounds.Width; }
		}

		public int Height
		{
			get { return bounds.Height; }
		}

		public Size Size
		{
			get { return bounds.Size; }
		}

		public Rectangle Bounds
		{
			get { return bounds; }
		}

		public bool Visible
		{
			get;
			set;
		}

		public bool IsFocusable
		{
			get;
			set;
		}

		public bool IsFocused
		{
			get { return isFocused; }
			set
			{
				if (isFocused == value)
					return;
				isFocused = value;
				OnFocusChanged();
			}
		}

		public bool IsHovered
		{
			get { return isHovered; }
			set
			{
				if (isHovered == value)
					return;
				isHovered = value;
				OnHoverChanged();
			}
		}

		public virtual MouseCursor Cursor
		{
			get { return Cursors.Default; }
		}

		#endregion

		#region Public Methods

		public Widget GetChildAt(Point p)
		{
			p = p.Sub(Location);
			foreach (var widget in ReversedChildren)
			{
				if (widget.Visible && widget.Bounds.Contains(p.X, p.Y))
					return widget.GetChildAt(p) ?? widget;
			}
			return null;
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
		}

		public void Dispose()
		{
			if (isDisposed)
				return;

			OnDispose();
			isDisposed = true;
		}

		public void Draw(DrawingContext dc)
		{
			dc.PushMatrix();
			dc.Translate(X, Y);
			// draw itself
			OnDraw(dc);
			// draw all children
			foreach (var widget in Children.Where(x => x.Visible))
				widget.Draw(dc);
			dc.PopMatrix();
		}

		public Widget SetLocation(int x, int y)
		{
			bounds.Location = new Point(x, y);
			return this;
		}

		public Widget SetLocation(Point p)
		{
			return SetLocation(p.X, p.Y);
		}

		public Widget SetSize(int width, int height)
		{
			bounds.Size = new Size(width, height);
			OnSizeChanged();
			return this;
		}

		#endregion

		#region Protected Methods

		protected Point PointToWidget(Point p)
		{
			return PointToWidget(p.X, p.Y);
		}

		protected Point PointToWidget(int x, int y)
		{
			for (var widget = this; widget != null; widget = widget.parent)
			{
				x -= widget.X;
				y -= widget.Y;
			}
			return new Point(x, y);
		}

		protected virtual void OnMouseButtonDown(MouseButtonEventArgs e)
		{
		}

		protected virtual void OnMouseButtonUp(MouseButtonEventArgs e)
		{
		}
		
		protected virtual void OnKeyDown(KeyEventArgs e)
		{
		}
		
		protected virtual void OnKeyUp(KeyEventArgs e)
		{
		}

		protected virtual void OnKeyPress(KeyPressEventArgs e)
		{
		}
		
		protected virtual void OnMouseMove(MouseMoveEventArgs e)
		{
		}

		protected virtual void OnMouseWheel(MouseWheelEventArgs e)
		{

		}
		
		protected virtual void OnDraw(DrawingContext dc)
		{
		}
		
		protected virtual void OnDispose()
		{
		}

		protected virtual void OnFocusChanged()
		{
		}

		protected virtual void OnHoverChanged()
		{
		}

		protected virtual void OnSizeChanged()
		{
		}

		#endregion

		private void AddChild(Widget child)
		{
			if (child.parent != null)
				child.parent.Remove(child);

			child.parent = this;

			if (this.firstChild == null)
				this.firstChild = child;

			if (this.lastChild != null)
			{
				child.previous = this.lastChild;
				this.lastChild.next = child;
				this.lastChild = child;
			}
			else
				this.lastChild = child;
		}

		#region IInputListener implementation

		public void MouseButtonDown(MouseButtonEventArgs e)
		{
			OnMouseButtonDown(e);
		}

		public void MouseButtonUp(MouseButtonEventArgs e)
		{
			OnMouseButtonUp(e);
		}

		public void MouseMove(MouseMoveEventArgs e)
		{
			OnMouseMove(e);
		}

		public void MouseWheel(MouseWheelEventArgs e)
		{
			OnMouseWheel(e);
		}

		public void KeyDown(KeyEventArgs e)
		{
			OnKeyDown(e);
			if (!e.Handled && parent != null)
				parent.KeyDown(e);

		}

		public void KeyUp(KeyEventArgs e)
		{
			OnKeyUp(e);
			if (!e.Handled && parent != null)
				parent.KeyUp(e);
		}

		public void KeyPress(KeyPressEventArgs e)
		{
			OnKeyPress(e);
			if (!e.Handled && parent != null)
				parent.KeyPress(e);
		}

		#endregion
	}
}

