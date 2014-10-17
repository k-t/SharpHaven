using System;
using System.Collections.Generic;
using System.Drawing;
using MonoHaven.Graphics;
using MonoHaven.UI.Widgets;
using OpenTK.Input;

namespace MonoHaven.UI
{
	public abstract class BaseScreen : IDisposable, IScreen
	{
		private readonly IScreenHost host;
		private readonly List<Widget> widgets = new List<Widget>();

		protected BaseScreen(IScreenHost host)
		{
			this.host = host;
		}

		protected IScreenHost Host
		{
			get { return host; }
		}

		protected virtual void OnShow() { }
		protected virtual void OnClose() { }
		protected virtual void OnResize(int newWidth, int newHeight) { }

		protected virtual void OnDraw(DrawingContext g)
		{
			foreach (var widget in widgets)
			{
				if (widget.Visible)
				{
					g.PushMatrix();
					g.Translate(widget.X, widget.Y);
					widget.Draw(g);
					g.PopMatrix();
				}
			}
		}

		protected virtual void OnMouseButtonDown(MouseButtonEventArgs e)
		{
			var widget = GetWidgetAt(e.Position);
			if (widget != null)
				widget.OnButtonDown(e);
		}

		protected virtual void OnMouseButtonUp(MouseButtonEventArgs e)
		{
			var widget = GetWidgetAt(e.Position);
			if (widget != null)
				widget.OnButtonUp(e);
		}

		protected virtual void OnMouseMove(MouseMoveEventArgs e)
		{
			var widget = GetWidgetAt(e.Position);
			if (widget != null)
				widget.OnMouseMove(e);
		}

		protected virtual void OnKeyDown(KeyboardKeyEventArgs e)
		{
			// TODO: focused widget
			var widget = widgets[0];
			if (widget != null)
				widget.OnKeyDown(e);
		}

		protected virtual void OnKeyUp(KeyboardKeyEventArgs e)
		{
			var widget = widgets[0];
			if (widget != null)
				widget.OnKeyUp(e);
		}

		protected void AddWidget(Widget widget)
		{
			this.widgets.Add(widget);
		}

		protected Widget GetWidgetAt(Point p)
		{
			for (int i = widgets.Count - 1; i >= 0; i--)
			{
				var widget = widgets[i];
				if (widget.Bounds.Contains(p.X, p.Y))
					return widget;
			}
			return null;
		}

		public virtual void Dispose()
		{
			foreach (var widget in widgets)
				widget.Dispose();
		}

		#region IScreen implementation

		void IScreen.Show()
		{
			this.OnShow();
		}

		void IScreen.Close()
		{
			this.OnClose();
		}

		void IScreen.Resize(int newWidth, int newHeight)
		{
			this.OnResize(newWidth, newHeight);
		}

		void IScreen.Draw(DrawingContext drawingContext)
		{
			this.OnDraw(drawingContext);
		}

		void IScreen.HandleMouseButtonDown(MouseButtonEventArgs e)
		{
			this.OnMouseButtonDown(e);
		}

		void IScreen.HandleMouseButtonUp(MouseButtonEventArgs e)
		{
			this.OnMouseButtonUp(e);
		}

		void IScreen.HandleMouseMove(MouseMoveEventArgs e)
		{
			this.OnMouseMove(e);
		}

		void IScreen.HandleKeyDown(KeyboardKeyEventArgs e)
		{
			this.OnKeyDown(e);
		}

		void IScreen.HandleKeyUp(KeyboardKeyEventArgs e)
		{
			this.OnKeyUp(e);
		}

		#endregion
	}
}
