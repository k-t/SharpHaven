using System;
using MonoHaven.Graphics;
using OpenTK.Input;

namespace MonoHaven.UI
{
	public class ImageButton : Widget
	{
		private bool isPressed;

		public Drawable Up { get; set; }
		public Drawable Down { get; set; }

		public event EventHandler Pressed;

		protected override void OnDraw(DrawingContext dc)
		{
			var tex = isPressed ? Down : Up;
			if (tex != null)
				dc.Draw(tex, 0, 0);
		}

		protected override void OnMouseButtonDown(MouseButtonEventArgs e)
		{
			GrabMouse();
			isPressed = true;
		}

		protected override void OnMouseButtonUp(MouseButtonEventArgs e)
		{
			ReleaseMouse();
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

