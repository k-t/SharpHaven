using System;
using System.Drawing;
using MonoHaven.Graphics;
using MonoHaven.Utils;
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

			// button released outside of borders?
			var p = PointToWidget(e.Position);
			if (Rectangle.FromLTRB(0, 0, Width, Height).Contains(p))
				Pressed.Raise(this, EventArgs.Empty);
		}
	}
}

