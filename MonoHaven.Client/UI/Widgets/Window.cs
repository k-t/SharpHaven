using System;
using System.Drawing;
using MonoHaven.Graphics;
using MonoHaven.Input;
using OpenTK.Input;

namespace MonoHaven.UI.Widgets
{
	public class Window : Widget
	{
		private static readonly Drawable box;
		private static readonly Drawable cap;
		private static readonly Drawable background;
		private static readonly Drawable closeButton;
		private static readonly Drawable closeButtonPressed;
		private static readonly Drawable closeButtonHovered;

		private bool dragging;
		private readonly ImageButton btnClose;
		private readonly TextLine titleLine;

		static Window()
		{
			background = App.Resources.Get<Drawable>("gfx/hud/bgtex");
			closeButton = App.Resources.Get<Drawable>("gfx/hud/cbtn");
			closeButtonPressed = App.Resources.Get<Drawable>("gfx/hud/cbtnd");
			closeButtonHovered = App.Resources.Get<Drawable>("gfx/hud/cbtnh");
			box = App.Resources.Get<Drawable>("custom/ui/wbox");
			cap = App.Resources.Get<Drawable>("custom/ui/wcap");
		}

		public Window(Widget parent) : this(parent, null)
		{
		}

		public Window(Widget parent, string title) : base(parent)
		{
			btnClose = new ImageButton(this);
			btnClose.Resize(closeButton.Size);
			btnClose.Image = closeButton;
			btnClose.PressedImage = closeButtonPressed;
			btnClose.HoveredImage = closeButtonHovered;
			btnClose.Click += () => Closed.Raise();

			if (!string.IsNullOrEmpty(title))
			{
				titleLine = new TextLine(Fonts.Text);
				titleLine.TextColor = Color.Yellow;
				titleLine.Append(title);
			}

			Margin = 20;
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
			}
			Resize(w + Margin * 2, h + Margin * 2);
		}

		protected override void OnDispose()
		{
			if (titleLine != null)
				titleLine.Dispose();
		}

		protected override void OnDraw(DrawingContext dc)
		{
			dc.Draw(background, 0, 0, Width, Height);
			dc.Draw(box, 0, 0, Width, Height);

			if (titleLine != null)
			{
				int hw = 48 + titleLine.TextWidth;
				dc.Draw(cap, (Width - hw) / 2, -7, hw, 21);
				dc.Draw(titleLine, (Width - titleLine.TextWidth) / 2, -5);
			}
		}

		protected override void OnMouseButtonDown(MouseButtonEvent e)
		{
			if (e.Button == MouseButton.Left)
			{
				dragging = true;
				Host.GrabMouse(this);
			}
			e.Handled = true;
		}

		protected override void OnMouseButtonUp(MouseButtonEvent e)
		{
			if (e.Button == MouseButton.Left)
			{
				dragging = false;
				Host.ReleaseMouse();
			}
			e.Handled = true;
		}

		protected override void OnMouseMove(MouseMoveEvent e)
		{
			if (dragging)
				Move(X + e.DeltaX, Y + e.DeltaY);
		}

		protected override void OnSizeChanged()
		{
			btnClose.Move(Width - Margin - closeButton.Width - 3, 3 - Margin);
		}
	}
}
