using System;
using MonoHaven.Graphics;
using MonoHaven.Input;
using MonoHaven.UI.Widgets;
using OpenTK;
using OpenTK.Input;

namespace MonoHaven.UI
{
	public class Scrollbar : Widget
	{
		private static readonly Drawable schain;
		private static readonly Drawable sflarp;

		static Scrollbar()
		{
			schain = App.Resources.GetImage("gfx/hud/schain");
			sflarp = App.Resources.GetImage("gfx/hud/sflarp");
		}

		private int value;
		private bool drag;

		public Scrollbar(Widget parent) : base(parent)
		{
			Resize(sflarp.Width, 0);
		}

		public event Action ValueChanged;

		public int Value
		{
			get { return value; }
			set
			{
				this.value = MathHelper.Clamp(value, Min, Max);
				ValueChanged.Raise();
			}
		}

		public int Min
		{
			get;
			set;
		}

		public int Max
		{
			get;
			set;
		}

		protected override void OnDraw(DrawingContext dc)
		{
			if (Max <= Min)
				return;

			dc.SetClip(0, 0, Width, Height);

			int cx = (sflarp.Width - schain.Width) / 2;
			for (int y = 0; y < Height; y += schain.Height - 1)
				dc.Draw(schain, cx, y);

			var a = (double)Value / (Max - Min);
			var fy = (int)((Height - sflarp.Height) * a);
			dc.Draw(sflarp, 0, fy);

			dc.ResetClip();
		}

		protected override void OnMouseButtonDown(MouseButtonEvent e)
		{
			if (e.Button == MouseButton.Left)
			{
				if (Max > Min)
				{
					SetScrollPosition(MapFromScreen(e.Position).Y);
					Host.GrabMouse(this);
					drag = true;
					e.Handled = true;
				}
			}
		}

		protected override void OnMouseMove(MouseMoveEvent e)
		{
			if (drag)
				SetScrollPosition(MapFromScreen(e.Position).Y);
		}

		protected override void OnMouseButtonUp(MouseButtonEvent e)
		{
			if (e.Button == MouseButton.Left)
			{
				if (drag)
				{
					Host.ReleaseMouse();
					drag = false;
					e.Handled = true;
				}
			}
		}

		private void SetScrollPosition(int y)
		{
			var a = (double)(y - (sflarp.Height / 2)) / (Height - sflarp.Height);
			a = MathHelper.Clamp(a, 0, 1);
			Value = (int)Math.Round(a * (Max - Min)) + Min;
		}
	}
}
