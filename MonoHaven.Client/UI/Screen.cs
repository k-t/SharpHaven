using System;
using System.Collections.Generic;
using System.Drawing;
using MonoHaven.Graphics;
using MonoHaven.UI.Widgets;
using OpenTK.Input;

namespace MonoHaven.UI
{
	public abstract class Screen : IDisposable
	{
		private readonly IScreenHost host;
		private readonly List<Widget> widgets = new List<Widget>();

		protected Screen(IScreenHost host)
		{
			this.host = host;
			this.host.Resized += (sender, args) => OnResize();
		}

		protected IScreenHost Host
		{
			get { return host; }
		}

		public virtual void Draw(DrawingContext g)
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

		public virtual void OnButtonDown(MouseButtonEventArgs e)
		{
			var widget = GetWidgetAt(e.Position);
			if (widget != null)
				widget.OnButtonDown(e);
		}

		public virtual void OnButtonUp(MouseButtonEventArgs e)
		{
			var widget = GetWidgetAt(e.Position);
			if (widget != null)
				widget.OnButtonUp(e);
		}

		public virtual void OnKeyDown(KeyboardKeyEventArgs e)
		{
			// TODO: focused widget
			var widget = widgets[0];
			if (widget != null)
				widget.OnKeyDown(e);
		}

		public virtual void OnKeyUp(KeyboardKeyEventArgs e)
		{
			var widget = widgets[0];
			if (widget != null)
				widget.OnKeyUp(e);
		}

		public virtual void OnMouseMove(MouseMoveEventArgs e)
		{
			var widget = GetWidgetAt(e.Position);
			if (widget != null)
				widget.OnMouseMove(e);
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

		protected virtual void OnResize() { }
	}
}
