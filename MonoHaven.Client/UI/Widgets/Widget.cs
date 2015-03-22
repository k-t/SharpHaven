using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using MonoHaven.Graphics;
using MonoHaven.Input;
using MonoHaven.Utils;
using OpenTK;

namespace MonoHaven.UI.Widgets
{
	public abstract class Widget : TreeNode<Widget>, IDisposable
	{
		private readonly IWidgetHost host;
		private Rectangle bounds;
		private bool isDisposed;
		private bool isFocused;
		private bool isHovered;
		private MouseCursor cursor;

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

		#region Events

		public event Action<KeyEvent> KeyDown;
		public event Action<KeyEvent> KeyUp;
		public event Action<KeyPressEvent> KeyPress;

		public event Action<MouseButtonEvent> MouseButtonDown;
		public event Action<MouseButtonEvent> MouseButtonUp;
		public event Action<MouseMoveEvent> MouseMove;
		public event Action<MouseWheelEvent> MouseWheel;

		#endregion

		#region Properties

		protected IWidgetHost Host
		{
			get { return host; }
		}

		private IEnumerable<Widget> ReversedChildren
		{
			get
			{
				for (var child = LastChild; child != null; child = child.Previous)
					yield return child;
			}
		}

		public int X
		{
			get { return bounds.X; }
		}

		public int Y
		{
			get { return bounds.Y; }
		}

		public Point Position
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

		public virtual MouseCursor Cursor
		{
			get { return cursor ?? (Parent != null ? Parent.Cursor : null); }
			set { cursor = value; }
		}

		public Rectangle Bounds
		{
			get { return bounds; }
		}

		public int Margin
		{
			get;
			set;
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

		public Tooltip Tooltip
		{
			get;
			set;
		}

		#endregion

		#region Public Methods

		public IEnumerable<Widget> GetChildrenAt(Point p)
		{
			var result = new List<Widget>();
			p = new Point(p.X - X - Margin, p.Y - Y - Margin);
			foreach (var widget in ReversedChildren)
			{
				if (widget.Visible)
				{
					result.AddRange(widget.GetChildrenAt(p));
					if (widget.CheckHit(p.X, p.Y))
						result.Add(widget);
				}
			}
			return result;
		}

		public Widget GetChildAt(Point p)
		{
			p = new Point(p.X - X - Margin, p.Y - Y - Margin);
			foreach (var widget in ReversedChildren)
			{
				if (widget.Visible)
				{
					var child = widget.GetChildAt(p);
					if (child != null) return child;
					if (widget.CheckHit(p.X, p.Y)) return widget;
				}
			}
			return null;
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
			dc.Translate(Margin, Margin);
			foreach (var widget in Children.Where(x => x.Visible))
				widget.Draw(dc);
			dc.PopMatrix();
		}

		public Widget Move(int x, int y)
		{
			bounds.Location = new Point(x, y);
			OnPositionChanged();
			return this;
		}

		public Widget Move(Point p)
		{
			return Move(p.X, p.Y);
		}

		public Widget Resize(int width, int height)
		{
			bounds.Size = new Size(width, height);
			OnSizeChanged();
			return this;
		}

		public Widget Resize(Size size)
		{
			return Resize(size.Width, size.Height);
		}

		public void HandleMouseButtonDown(MouseButtonEvent e)
		{
			OnMouseButtonDown(e);
			if (!e.Handled && Parent != null)
				Parent.OnMouseButtonDown(e);
		}

		public void HandleMouseButtonUp(MouseButtonEvent e)
		{
			OnMouseButtonUp(e);
			if (!e.Handled && Parent != null)
				Parent.OnMouseButtonUp(e);
		}

		public void HandleMouseMove(MouseMoveEvent e)
		{
			OnMouseMove(e);
		}

		public void HandleMouseWheel(MouseWheelEvent e)
		{
			OnMouseWheel(e);
			if (!e.Handled && Parent != null)
				Parent.HandleMouseWheel(e);
		}

		public void HandleKeyDown(KeyEvent e)
		{
			OnKeyDown(e);
			if (!e.Handled && Parent != null)
				Parent.HandleKeyDown(e);
		}

		public void HandleKeyUp(KeyEvent e)
		{
			OnKeyUp(e);
			if (!e.Handled && Parent != null)
				Parent.HandleKeyUp(e);
		}

		public void HandleKeyPress(KeyPressEvent e)
		{
			OnKeyPress(e);
			if (!e.Handled && Parent != null)
				Parent.HandleKeyPress(e);
		}

		#endregion

		#region Protected Methods

		public Point MapFromScreen(Point p)
		{
			return MapFromScreen(p.X, p.Y);
		}

		public Point MapFromScreen(int x, int y)
		{
			for (var widget = this; widget != null; widget = widget.Parent)
			{
				x -= (widget.X + widget.Margin);
				y -= (widget.Y + widget.Margin);
			}
			return new Point(x, y);
		}

		protected virtual bool CheckHit(int x, int y)
		{
			return Bounds.Contains(x, y);
		}

		protected virtual void OnMouseButtonDown(MouseButtonEvent e)
		{
			MouseButtonDown.Raise(e);
		}

		protected virtual void OnMouseButtonUp(MouseButtonEvent e)
		{
			MouseButtonUp.Raise(e);
		}

		protected virtual void OnMouseMove(MouseMoveEvent e)
		{
			MouseMove.Raise(e);
		}

		protected virtual void OnMouseWheel(MouseWheelEvent e)
		{
			MouseWheel.Raise(e);
		}
		
		protected virtual void OnKeyDown(KeyEvent e)
		{
			KeyDown.Raise(e);
		}

		protected virtual void OnKeyUp(KeyEvent e)
		{
			KeyUp.Raise(e);
		}

		protected virtual void OnKeyPress(KeyPressEvent e)
		{
			KeyPress.Raise(e);
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

		protected virtual void OnPositionChanged()
		{
		}

		protected virtual void OnSizeChanged()
		{
		}

		#endregion
	}
}

