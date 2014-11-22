#region License
// Copyright (c) 2014 Marat Vildanov <marat.vildanov@gmail.com>
// Distributed under the MIT License.
// (See accompanying LICENSE file or copy at http://opensource.org/licenses/MIT)
#endregion

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
	public abstract class Widget : TreeNode<Widget>, IDisposable
	{
		private readonly IWidgetHost host;
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

		public virtual Widget SetLocation(int x, int y)
		{
			bounds.Location = new Point(x, y);
			return this;
		}

		public virtual Widget SetLocation(Point p)
		{
			return SetLocation(p.X, p.Y);
		}

		public virtual Widget SetSize(int width, int height)
		{
			bounds.Size = new Size(width, height);
			OnSizeChanged();
			return this;
		}

		public void HandleMouseButtonDown(MouseButtonEventArgs e)
		{
			OnMouseButtonDown(e);
		}

		public void HandleMouseButtonUp(MouseButtonEventArgs e)
		{
			OnMouseButtonUp(e);
		}

		public void HandleMouseMove(MouseMoveEventArgs e)
		{
			OnMouseMove(e);
		}

		public bool HandleMouseWheel(MouseWheelEventArgs e)
		{
			return OnMouseWheel(e) || (Parent != null && Parent.HandleMouseWheel(e));
		}

		public bool HandleKeyDown(KeyboardKeyEventArgs e)
		{
			return OnKeyDown(e) || (Parent != null && Parent.HandleKeyDown(e));
		}

		public bool HandleKeyUp(KeyboardKeyEventArgs e)
		{
			return OnKeyUp(e) || (Parent != null && Parent.HandleKeyUp(e));
		}

		public bool HandleKeyPress(KeyPressEventArgs e)
		{
			return OnKeyPress(e) || (Parent != null && Parent.HandleKeyPress(e));
		}

		#endregion

		#region Protected Methods

		protected Point PointToWidget(Point p)
		{
			return PointToWidget(p.X, p.Y);
		}

		protected Point PointToWidget(int x, int y)
		{
			for (var widget = this; widget != null; widget = widget.Parent)
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
		
		protected virtual bool OnKeyDown(KeyboardKeyEventArgs e)
		{
			return false;
		}
		
		protected virtual bool OnKeyUp(KeyboardKeyEventArgs e)
		{
			return false;
		}

		protected virtual bool OnKeyPress(KeyPressEventArgs e)
		{
			return false;
		}
		
		protected virtual void OnMouseMove(MouseMoveEventArgs e)
		{
		}

		protected virtual bool OnMouseWheel(MouseWheelEventArgs e)
		{
			return false;
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
	}
}

