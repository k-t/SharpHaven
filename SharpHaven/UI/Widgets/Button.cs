using System;
using System.Drawing;
using OpenTK.Input;
using SharpHaven.Graphics;
using SharpHaven.Graphics.Text;
using SharpHaven.Input;

namespace SharpHaven.UI.Widgets
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

		private readonly TextLine textLine;
		private string text;
		private bool isPressed;

		static Button()
		{
			bl = App.Resources.Get<Drawable>("gfx/hud/buttons/tbtn/left");
			br = App.Resources.Get<Drawable>("gfx/hud/buttons/tbtn/right");
			bt = App.Resources.Get<Drawable>("gfx/hud/buttons/tbtn/top");
			bb = App.Resources.Get<Drawable>("gfx/hud/buttons/tbtn/bottom");
			dt = App.Resources.Get<Drawable>("gfx/hud/buttons/tbtn/dtex");
			ut = App.Resources.Get<Drawable>("gfx/hud/buttons/tbtn/utex");
		}

		public Button(Widget parent, int width) : base(parent)
		{
			textLine = new TextLine(Fonts.Text);
			textLine.TextColor = Color.Yellow;
			textLine.TextAlign = TextAlign.Center;
			this.Resize(width, ButtonHeight);
		}

		public event Action Click;

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
				textLine.Clear();
				textLine.Append(value);
			}
		}

		protected override void OnDispose()
		{
			textLine.Dispose();
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
			dc.Draw(textLine, offset, offset + 2);
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
				Click.Raise();

			e.Handled = true;
		}

		protected override void OnSizeChanged()
		{
			textLine.SetWidth(Width);
		}
	}
}
