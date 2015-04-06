using System;
using MonoHaven.Game;
using MonoHaven.Graphics;
using MonoHaven.Input;
using MonoHaven.Utils;
using OpenTK.Input;

namespace MonoHaven.UI.Widgets
{
	public class MenuButton : Widget
	{
		private static readonly Drawable background;

		private bool isPressed;

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
			switch (e.Button)
			{
				case MouseButton.Left:
					isPressed = true;
					Host.GrabMouse(this);
					break;
			}
		}

		protected override void OnMouseButtonUp(MouseButtonEvent e)
		{
			switch (e.Button)
			{
				case MouseButton.Left:
					isPressed = false;
					Host.ReleaseMouse();
					Click.Raise(this, e);
					break;
			}
		}
	}
}
