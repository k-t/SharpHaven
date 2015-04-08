using System;
using System.Drawing;
using MonoHaven.Graphics;
using MonoHaven.Input;
using MonoHaven.UI.Widgets;
using OpenTK;
using OpenTK.Input;

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
		private Drag drag;

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

			if (drag != null)
				dc.Draw(drag.Image,
					mousePosition.X - drag.Image.Width / 2,
					mousePosition.Y - drag.Image.Height / 2);
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
			if (e.Button == MouseButton.Right && drag != null)
				drag = null;

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
			if (drag != null)
			{
				var dropTarget = RootWidget.GetChildAt(e.Position);
				if (dropTarget != null)
					dropTarget.HandleDrop(new DropEvent(e.Position, drag.Tag));
				drag = null;
			}
			else
			{
				var widget = mouseFocus ?? RootWidget.GetChildAt(e.Position);
				if (widget != null)
					widget.HandleMouseButtonUp(e);
			}
		}

		void IScreen.MouseMove(MouseMoveEvent e)
		{
			mousePosition = e.Position;

			var widget = mouseFocus ?? RootWidget.GetChildAt(e.Position);
			SetHoveredWidget(widget);
			if (widget != null)
				widget.HandleMouseMove(e);
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

		void IWidgetHost.DoDragDrop(Drag drag)
		{
			this.drag = drag;
		}

		#endregion
	}
}
