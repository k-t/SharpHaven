using System;
using MonoHaven.Graphics;
using MonoHaven.Input;

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

		public event Action Clicked;

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

		public Drawable HoveredImage
		{
			get;
			set;
		}

		protected override void OnDraw(DrawingContext dc)
		{
			Drawable image = null;

			if (isPressed && PressedImage != null)
				image = PressedImage;
			else if (IsHovered && HoveredImage != null)
				image = HoveredImage;
			else
				image = Image;

			if (image != null)
				dc.Draw(image, 0, 0);
		}

		protected override bool CheckHit(int x, int y)
		{
			return Image != null && Image.CheckHit(x - X, y - Y);
		}

		protected override void OnMouseButtonDown(MouseButtonEvent e)
		{
			Host.GrabMouse(this);
			isPressed = true;
			e.Handled = true;
		}

		protected override void OnMouseButtonUp(MouseButtonEvent e)
		{
			Host.ReleaseMouse();
			isPressed = false;

			// button released outside of borders?
			var p = MapFromScreen(e.Position);
			if (CheckHit(p.X + X, p.Y + Y))
				Clicked.Raise();

			e.Handled = true;
		}
	}
}

