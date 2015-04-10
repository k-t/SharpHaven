using System;
using System.Drawing;
using OpenTK.Input;
using SharpHaven.Graphics;
using SharpHaven.Input;
using SharpHaven.Utils;

namespace SharpHaven.UI.Widgets
{
	public class MenuButton : Widget
	{
		private static readonly Drawable background;

		private bool isPressed;
		private Point dragPosition;

		static MenuButton()
		{
			background = App.Resources.Get<Drawable>("gfx/hud/invsq");
		}

		public MenuButton(Widget parent) : base(parent)
		{
			Resize(background.Size);
		}

		public event EventHandler<MouseButtonEvent> Click;

		public Drawable Image
		{
			get;
			set;
		}

		public MenuNode Node
		{
			get;
			set;
		}

		protected override void OnDraw(DrawingContext dc)
		{
			dc.Draw(background, 0, 0);
			if (Image != null)
			{
				dc.Draw(Image, 1, 1, Width - 2, Height - 2);
				if (isPressed)
				{
					dc.SetColor(0, 0, 0, 128);
					dc.DrawRectangle(1, 1, Width - 2, Height - 2);
					dc.ResetColor();
				}
			}
		}

		protected override void OnMouseButtonDown(MouseButtonEvent e)
		{
			if (e.Button == MouseButton.Left)
			{
				isPressed = true;
				dragPosition = e.Position;
				Host.GrabMouse(this);
			}
		}

		protected override void OnMouseButtonUp(MouseButtonEvent e)
		{
			if (e.Button == MouseButton.Left)
			{
				if (isPressed)
				{
					isPressed = false;
					Host.ReleaseMouse();
					Click.Raise(this, e);
				}
			}
		}

		protected override void OnMouseMove(MouseMoveEvent e)
		{
			if (isPressed && dragPosition.DistanceTo(e.Position) > 2)
			{
				if (Node != null && Node.Tag != null)
				{
					isPressed = false;
					Host.ReleaseMouse();
					Host.DoDragDrop(new Drag(Node.Image, Node.Tag));
				}
			}
		}
	}
}
