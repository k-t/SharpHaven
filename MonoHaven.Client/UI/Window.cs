using System;
using System.Drawing;
using MonoHaven.Graphics;
using MonoHaven.Resources;
using MonoHaven.Utils;
using OpenTK.Input;

namespace MonoHaven.UI
{
	public class Window : Widget
	{
		private static readonly Drawable box;
		private static readonly Drawable cap;
		private static readonly Drawable background;
		private static readonly Drawable closeButton;
		private static readonly Drawable closeButtonPressed;
		private static readonly Drawable closeButtonHovered;

		private static readonly Point margin = new Point(12, 12);

		private bool dragging;
		private readonly ImageButton btnClose;
		private readonly TextBlock tblCaption;

		static Window()
		{
			background = App.Instance.Resources.GetTexture("gfx/hud/bgtex");
			closeButton = App.Instance.Resources.GetTexture("gfx/hud/cbtn");
			closeButtonPressed = App.Instance.Resources.GetTexture("gfx/hud/cbtnd");
			closeButtonHovered = App.Instance.Resources.GetTexture("gfx/hud/cbtnh");
			using (var bitmap = EmbeddedResource.GetImage("wbox.png"))
				box = new NinePatch(new Texture(bitmap), 8, 8, 8, 8);
			using (var bitmap = EmbeddedResource.GetImage("wcap.png"))
				cap = new NinePatch(new Texture(bitmap), 24, 24, 0, 0);
		}

		public Window(Widget parent, string caption) : base(parent)
		{
			btnClose = new ImageButton(this);
			btnClose.SetSize(closeButton.Width, closeButton.Height);
			btnClose.Image = closeButton;
			btnClose.PressedImage = closeButtonPressed;
			btnClose.HoveredImage = closeButtonHovered;
			btnClose.Clicked += () => Closed.Raise();

			tblCaption = new TextBlock(Fonts.Text);
			tblCaption.TextColor = Color.Yellow;
			tblCaption.Append(caption);
		}

		public event Action Closed;

		public void Pack()
		{
			int w = 0;
			int h = 0;
			foreach (var child in Children)
			{
				if (child == btnClose)
					continue;
				w = Math.Max(w, child.X + child.Width);
				h = Math.Max(h, child.Y + child.Height);
				// FIXME: temporary solution to properly place widgets
				child.X += margin.X + 8;
				child.Y += margin.Y + 8;
			}
			SetSize(w + margin.X * 2 + 16, h + margin.Y * 2 + 16);
		}

		protected override void OnDispose()
		{
			tblCaption.Dispose();
		}

		protected override void OnDraw(DrawingContext dc)
		{
			dc.Draw(background, 0, 0, Width, Height);
			dc.Draw(box, 0, 0, Width, Height);

			int hw = 48 + tblCaption.TextWidth;
			dc.Draw(cap, (Width - hw) / 2, -7, hw, 21);
			dc.Draw(tblCaption, (Width - tblCaption.TextWidth) / 2, -5);
		}

		protected override void OnMouseButtonDown(MouseButtonEventArgs e)
		{
			if (e.Button == MouseButton.Left)
			{
				dragging = true;
				Host.GrabMouse(this);
			}
		}

		protected override void OnMouseButtonUp(MouseButtonEventArgs e)
		{
			if (e.Button == MouseButton.Left)
			{
				dragging = false;
				Host.ReleaseMouse();
			}
		}

		protected override void OnMouseMove(MouseMoveEventArgs e)
		{
			if (dragging)
				SetLocation(X + e.XDelta, Y + e.YDelta);
		}

		protected override void OnSizeChanged()
		{
			btnClose.SetLocation(Width - closeButton.Width - 3, 3);
		}
	}
}
