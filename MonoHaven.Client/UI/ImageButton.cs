using System;
using MonoHaven.Graphics;
using OpenTK.Input;

namespace MonoHaven.UI
{
	public class ImageButton : Widget
	{
		private bool isPressed;

		public ImageButton(Widget parent)
			: base(parent)
		{
			IsFocusable = true;
		}

		public event EventHandler Pressed;

		public Drawable Image
		{
			get;
			set;
		}

		public Drawable PressedImage
		{
			get;
			set;
		}

		protected override void OnDraw(DrawingContext dc)
		{
			var tex = isPressed ? PressedImage : Image;
			if (tex != null)
				dc.Draw(tex, 0, 0);
		}

		protected override void OnMouseButtonDown(MouseButtonEventArgs e)
		{
			Host.GrabMouse(this);
			isPressed = true;
		}

		protected override void OnMouseButtonUp(MouseButtonEventArgs e)
		{
			Host.ReleaseMouse();
			isPressed = false;
			RaisePressedEvent();
		}

		private void RaisePressedEvent()
		{
			if (Pressed != null)
				Pressed(this, EventArgs.Empty);
		}
	}
}

