using System;
using Haven;
using SharpHaven.Graphics;
using SharpHaven.Input;

namespace SharpHaven.UI.Widgets
{
	public class ImageButton : Widget
	{
		private bool isPressed;
		private Drawable image;

		public ImageButton(Widget parent)
			: base(parent)
		{
			IsFocusable = true;
		}

		public event Action Click;

		public bool ImageHitTest
		{
			get;
			set;
		}

		public Drawable Image
		{
			get { return image; }
			set
			{
				image = value;
				UpdateSize();
			}
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
			if (ImageHitTest)
				return Image != null && Image.CheckHit(x - X, y - Y);
			return base.CheckHit(x, y);
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
				Click.Raise();

			e.Handled = true;
		}

		private void UpdateSize()
		{
			Size = Image?.Size ?? Point2D.Empty;
		}
	}
}

