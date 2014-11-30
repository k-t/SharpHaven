using System;
using System.Drawing;
using MonoHaven.Graphics;
using MonoHaven.Input;
using OpenTK.Input;

namespace MonoHaven.UI
{
	public class Button : Widget
	{
		private const int ButtonHeight = 19;

		private static readonly Drawable bl;
		private static readonly Drawable br;
		private static readonly Drawable bt;
		private static readonly Drawable bb;
		private static readonly Drawable dt;
		private static readonly Drawable ut;

		private readonly TextBlock textBlock;
		private string text;
		private bool isPressed;

		static Button()
		{
			bl = App.Resources.GetImage("gfx/hud/buttons/tbtn/left");
			br = App.Resources.GetImage("gfx/hud/buttons/tbtn/right");
			bt = App.Resources.GetImage("gfx/hud/buttons/tbtn/top");
			bb = App.Resources.GetImage("gfx/hud/buttons/tbtn/bottom");
			dt = App.Resources.GetImage("gfx/hud/buttons/tbtn/dtex");
			ut = App.Resources.GetImage("gfx/hud/buttons/tbtn/utex");
		}

		public Button(Widget parent, int width) : base(parent)
		{
			Resize(width, ButtonHeight);
			textBlock = new TextBlock(Fonts.Text);
			textBlock.TextColor = Color.Yellow;
			textBlock.TextAlign = TextAlign.Center;
			textBlock.SetWidth(width);
		}

		public event Action Clicked;

		public Drawable Image
		{
			get;
			set;
		}

		public string Text
		{
			get { return text; }
			set
			{
				text = value;
				textBlock.Clear();
				textBlock.Append(value);
			}
		}

		protected override void OnDispose()
		{
			textBlock.Dispose();
		}

		protected override void OnDraw(DrawingContext dc)
		{
			// draw borders
			dc.Draw(isPressed ? dt : ut, 3, 3, Width - 6, 13);
			dc.Draw(bl, 0, 0);
			dc.Draw(br, Width - br.Width, 0);
			dc.Draw(bt, 3, 0, Width - 6, bt.Height);
			dc.Draw(bb, 3, Height - bb.Height, Width - 6, bb.Height);
			// draw content
			int offset = isPressed ? 1 : 0;
			if (Image != null)
			{
				var p = new Point(
					(Width - Image.Width) / 2 + offset,
					(Height - Image.Height) / 2 + offset);
				dc.Draw(Image, p);
			}
			dc.Draw(textBlock, offset, offset + 2);
		}

		protected override void OnMouseButtonDown(MouseButtonEvent e)
		{
			if (e.Button != MouseButton.Left)
				return;
			Host.GrabMouse(this);
			isPressed = true;
			e.Handled = true;
		}

		protected override void OnMouseButtonUp(MouseButtonEvent e)
		{
			if (e.Button != MouseButton.Left)
				return;

			Host.ReleaseMouse();
			isPressed = false;

			// button released outside of borders?
			var p = MapFromScreen(e.Position);
			if (Rectangle.FromLTRB(0, 0, Width, Height).Contains(p))
				Clicked.Raise();

			e.Handled = true;
		}
	}
}
