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

		protected virtual void OnShow() {}

		protected virtual void OnClose() {}

		protected virtual void OnResize(int newWidth, int newHeight) {}

		protected virtual void OnDraw(DrawingContext dc)
		{
			rootWidget.Draw(dc);
		}

		private void SetKeyboardFocus(Widget widget)
		{
			if (keyboardFocus != null)
				keyboardFocus.IsFocused = false;

			keyboardFocus = widget;

			if (keyboardFocus != null)
				keyboardFocus.IsFocused = true;
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

		void IScreen.Draw(DrawingContext dc)
		{
			this.OnDraw(dc);
		}

		#endregion

		#region IInputListener implementation

		void IInputListener.MouseButtonDown(MouseButtonEventArgs e)
		{
			var widget = mouseFocus ?? RootWidget.GetChildAt(e.Position);
			if (widget != null)
			{
				if (widget != keyboardFocus && widget.IsFocusable)
					SetKeyboardFocus(widget);
				
				((IInputListener)widget).MouseButtonDown(e);
			}
		}

		void IInputListener.MouseButtonUp(MouseButtonEventArgs e)
		{
			IInputListener widget = mouseFocus ?? RootWidget.GetChildAt(e.Position);
			if (widget != null)
				widget.MouseButtonUp(e);
		}

		void IInputListener.MouseMove(MouseMoveEventArgs e)
		{
			IInputListener widget = mouseFocus ?? RootWidget.GetChildAt(e.Position);
			if (widget != null)
				widget.MouseMove(e);
		}

		void IInputListener.KeyDown(KeyboardKeyEventArgs e)
		{
			IInputListener widget = keyboardFocus;
			if (widget != null) widget.KeyDown(e);
		}

		void IInputListener.KeyUp(KeyboardKeyEventArgs e)
		{
			IInputListener widget = keyboardFocus;
			if (widget != null) widget.KeyUp(e);
		}

		#endregion

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
		}
	}
}
