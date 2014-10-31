using System;
using MonoHaven.Graphics;
using OpenTK.Input;

namespace MonoHaven.UI
{
	public abstract class BaseScreen : IDisposable, IScreen, IWidgetHost
	{
		private readonly IScreenHost host;
		private readonly RootWidget rootWidget;
		private Widget mouseFocus;
		private Widget keyboardFocus;
		private Widget hoveredWidget;

		protected BaseScreen(IScreenHost host)
		{
			this.rootWidget = new RootWidget(this);
			this.host = host;
		}

		public virtual void Dispose()
		{
			rootWidget.Dispose();
		}

		protected IScreenHost Host
		{
			get { return host; }
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
		}

		protected virtual void OnDraw(DrawingContext dc)
		{
			rootWidget.Draw(dc);
		}

		protected virtual void OnKeyDown(KeyEventArgs args)
		{
		}

		protected virtual void OnKeyUp(KeyEventArgs args)
		{
		}

		protected virtual void OnKeyPress(KeyPressEventArgs args)
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
			if (hoveredWidget != null)
			{
				host.SetCursor(hoveredWidget.Cursor);
				hoveredWidget.IsHovered = true;
			}
		}

		#region IScreen Implementation

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

		#endregion

		#region IInputListener Implementation

		void IInputListener.MouseButtonDown(MouseButtonEventArgs e)
		{
			var widget = mouseFocus ?? RootWidget.GetChildAt(e.Position);
			if (widget != null)
			{
				if (widget != keyboardFocus && widget.IsFocusable)
					SetKeyboardFocus(widget);
				widget.MouseButtonDown(e);
			}
		}

		void IInputListener.MouseButtonUp(MouseButtonEventArgs e)
		{
			var widget = mouseFocus ?? RootWidget.GetChildAt(e.Position);
			if (widget != null)
				widget.MouseButtonUp(e);
		}

		void IInputListener.MouseMove(MouseMoveEventArgs e)
		{
			if (mouseFocus != null)
				// don't hover widgets mouse is grabbed
				((IInputListener)mouseFocus).MouseMove(e);
			else
			{
				var widget = mouseFocus ?? RootWidget.GetChildAt(e.Position);
				SetHoveredWidget(widget);
				if (widget != null)
					widget.MouseMove(e);
			}
		}

		void IInputListener.KeyDown(KeyEventArgs e)
		{
			var widget = keyboardFocus;
			if (widget != null) widget.KeyDown(e);
			if (!e.Handled) OnKeyDown(e);
		}

		void IInputListener.KeyUp(KeyEventArgs e)
		{
			var widget = keyboardFocus;
			if (widget != null) widget.KeyUp(e);
			if (!e.Handled) OnKeyUp(e);
		}

		void IInputListener.KeyPress(KeyPressEventArgs e)
		{
			var widget = keyboardFocus;
			if (widget != null) widget.KeyPress(e);
			if (!e.Handled) OnKeyPress(e);
		}

		#endregion

		#region IWidgetHost Implementation

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
