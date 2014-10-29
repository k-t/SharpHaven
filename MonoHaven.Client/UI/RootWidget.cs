using OpenTK.Input;

namespace MonoHaven.UI
{
	public class RootWidget : Widget
	{
		private Widget mouseFocus;
		private Widget keyboardFocus;

		public RootWidget()
		{
			IsFocusable = true;
		}

		// TODO: ugly!1!!
		protected override RootWidget Root
		{
			get { return this; }
		}

		public Widget KeyboardFocus
		{
			get { return keyboardFocus; }
		}

		public void SetKeyboardFocus(Widget widget)
		{
			keyboardFocus = widget;
		}

		public void SetMouseFocus(Widget widget)
		{
			mouseFocus = (widget != this) ? widget : null;
		}

		protected override void OnMouseButtonDown(MouseButtonEventArgs e)
		{
			IInputListener widget = mouseFocus ?? GetChildAt(e.Position);
			if (widget != null)
				widget.MouseButtonDown(e);
		}

		protected override void OnMouseButtonUp(MouseButtonEventArgs e)
		{
			IInputListener widget = mouseFocus ?? GetChildAt(e.Position);
			if (widget != null)
				widget.MouseButtonUp(e);
		}

		protected override void OnMouseMove(MouseMoveEventArgs e)
		{
			IInputListener widget = mouseFocus ?? GetChildAt(e.Position);
			if (widget != null)
				widget.MouseMove(e);
		}

		protected override void OnKeyDown(KeyboardKeyEventArgs e)
		{
			IInputListener widget = keyboardFocus;
			if (widget != null)
				widget.KeyDown(e);
		}

		protected override void OnKeyUp(KeyboardKeyEventArgs e)
		{
			IInputListener widget = keyboardFocus;
			if (widget != null)
				widget.KeyUp(e);
		}
	}
}
