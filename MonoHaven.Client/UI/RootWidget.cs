using OpenTK.Input;

namespace MonoHaven.UI
{
	public class RootWidget : Widget
	{
		private IInputListener mouseFocus;

		protected override RootWidget Root
		{
			get { return this; }
		}

		public void GrabMouse(Widget widget)
		{
			mouseFocus = (widget != this) ? widget : null;
		}

		protected override void OnMouseButtonDown(MouseButtonEventArgs e)
		{
			var widget = mouseFocus ?? GetChildAt(e.Position);
			if (widget != null)
				widget.MouseButtonDown(e);
		}

		protected override void OnMouseButtonUp(MouseButtonEventArgs e)
		{
			var widget = mouseFocus ?? GetChildAt(e.Position);
			if (widget != null)
				widget.MouseButtonUp(e);
		}

		protected override void OnMouseMove(MouseMoveEventArgs e)
		{
			var widget = mouseFocus ?? GetChildAt(e.Position);
			if (widget != null)
				widget.MouseMove(e);
		}
	}
}
