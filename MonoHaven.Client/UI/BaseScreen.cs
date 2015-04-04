using System;
using System.Drawing;
using MonoHaven.Graphics;
using MonoHaven.Input;
using MonoHaven.UI.Widgets;
using OpenTK;

namespace MonoHaven.UI
{
	public abstract class BaseScreen : IDisposable, IScreen, IWidgetHost
	{
		private readonly RootWidget rootWidget;
		protected Point mousePosition;
		protected Widget mouseFocus;
		protected Widget keyboardFocus;
		private Widget hoveredWidget;
		private Tooltip tooltip;

		protected BaseScreen()
		{
			rootWidget = new RootWidget(this);
		}

		private Tooltip Tooltip
		{
			get { return tooltip; }
			set
			{
				if (tooltip != value)
				{
					if (tooltip != null)
						tooltip.Dispose();
					tooltip = value;
				}
			}
		}

		public virtual void Dispose()
		{
			rootWidget.Dispose();
		}

		protected INativeWindow Window
		{
			get { return App.Window; }
		}

		protected RootWidget RootWidget
		{
			get { return rootWidget; }
		}

		protected virtual void OnShow()
		{
		}

		protected virtual void OnClose()
		{
		}

		protected virtual void OnResize(int newWidth, int newHeight)
		{
			RootWidget.Resize(newWidth, newHeight);
		}

		protected virtual void OnDraw(DrawingContext dc)
		{
			if (hoveredWidget != null)
			{
				var cursor = hoveredWidget.Cursor ?? Cursors.Default;
				if (Window.Cursor != cursor)
					Window.Cursor = cursor;
				Tooltip = hoveredWidget.Tooltip;
			}
			else
			{
				Window.Cursor = Cursors.Default;
				Tooltip = null;
			}

			rootWidget.Draw(dc);

			if (tooltip != null)
				tooltip.Draw(dc, mousePosition.X, mousePosition.Y);
		}

		protected virtual void OnUpdate(int dt)
		{
		}

		protected void SetKeyboardFocus(Widget widget)
		{
			if (keyboardFocus != null) keyboardFocus.IsFocused = false;
			keyboardFocus = widget;
			if (keyboardFocus != null) keyboardFocus.IsFocused = true;
		}

		private void SetHoveredWidget(Widget widget)
		{
			if (hoveredWidget == widget) return;
			if (hoveredWidget != null) hoveredWidget.IsHovered = false;
			hoveredWidget = widget;
			if (hoveredWidget != null) hoveredWidget.IsHovered = true;
		}

		#region IScreen

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

		void IScreen.Draw(DrawingContext dc)
		{
			this.OnDraw(dc);
		}

		void IScreen.Update(int dt)
		{
			this.OnUpdate(dt);
		}

		void IScreen.MouseButtonDown(MouseButtonEvent e)
		{
			var widget = mouseFocus ?? RootWidget.GetChildAt(e.Position);
			if (widget != null)
			{
				if (widget != keyboardFocus && widget.IsFocusable)
					SetKeyboardFocus(widget);
				widget.HandleMouseButtonDown(e);
			}
		}

		void IScreen.MouseButtonUp(MouseButtonEvent e)
		{
			var widget = mouseFocus ?? RootWidget.GetChildAt(e.Position);
			if (widget != null)
				widget.HandleMouseButtonUp(e);
		}

		void IScreen.MouseMove(MouseMoveEvent e)
		{
			mousePosition = e.Position;

			if (mouseFocus != null)
				// don't hover widgets mouse is grabbed
				mouseFocus.HandleMouseMove(e);
			else
			{
				var widget = mouseFocus ?? RootWidget.GetChildAt(e.Position);
				SetHoveredWidget(widget);
				if (widget != null)
					widget.HandleMouseMove(e);
			}
		}

		void IScreen.MouseWheel(MouseWheelEvent e)
		{
			var widget = mouseFocus ?? hoveredWidget;
			if (widget != null) widget.HandleMouseWheel(e);
		}

		void IScreen.KeyDown(KeyEvent e)
		{
			var widget = keyboardFocus ?? rootWidget;
			if (widget != null) widget.HandleKeyDown(e);
		}

		void IScreen.KeyUp(KeyEvent e)
		{
			var widget = keyboardFocus ?? rootWidget;
			if (widget != null) widget.HandleKeyUp(e);
		}

		void IScreen.KeyPress(KeyPressEvent e)
		{
			var widget = keyboardFocus;
			if (widget != null) widget.HandleKeyPress(e);
		}

		#endregion

		#region IWidgetHost

		public Point MousePosition
		{
			get { return mousePosition; }
		}

		void IWidgetHost.RequestKeyboardFocus(Widget widget)
		{
			SetKeyboardFocus(widget);
		}

		void IWidgetHost.GrabMouse(Widget widget)
		{
			mouseFocus = widget;
		}

		void IWidgetHost.ReleaseMouse()
		{
			mouseFocus = null;
			SetHoveredWidget(null);
		}

		#endregion
	}
}
