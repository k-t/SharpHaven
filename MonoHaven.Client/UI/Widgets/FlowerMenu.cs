using System;
using System.Collections.Generic;
using System.Drawing;
using MonoHaven.Graphics;
using MonoHaven.Graphics.Text;
using MonoHaven.Input;
using OpenTK.Input;

namespace MonoHaven.UI.Widgets
{
	public class FlowerMenu : Widget
	{
		private static readonly Drawable box;
		private static readonly Drawable back;

		static FlowerMenu()
		{
			back = App.Resources.Get<Drawable>("gfx/hud/bgtex");
			box = App.Resources.Get<Drawable>("custom/ui/wbox");
		}

		private readonly int optionCount;

		public FlowerMenu(Widget parent, IEnumerable<string> options) : base(parent)
		{
			int n = 0;
			foreach (var option in options)
			{
				var petal = new Petal(this, option, n++);
				petal.Move(0, (n - 1) * 25);
			}
			optionCount = n;

			IsFocusable = true;
			Host.GrabMouse(this);
			Host.RequestKeyboardFocus(this);
		}

		public event Action<int> Selected;

		protected override void OnMouseButtonDown(MouseButtonEvent e)
		{
			int num = -1;
			if (e.Button == MouseButton.Left)
			{
				var petal = GetChildAt(e.Position) as Petal;
				if (petal != null)
					num = petal.Num;
			}
			Select(num);
			e.Handled = true;
		}

		protected override void OnKeyPress(KeyPressEvent e)
		{
			if (char.IsNumber(e.KeyChar))
			{
				int option = (e.KeyChar == '0') ? 10 : (e.KeyChar - '1');
				if (option < optionCount)
					Select(option);
				e.Handled = true;
			}
		}

		protected override void OnKeyDown(KeyEvent e)
		{
			if (e.Key == Key.Escape)
			{
				Select(-1);
				e.Handled = true;
			}
		}

		protected override void OnPositionChanged()
		{
			// TODO: fix potential stack overflow
			if (Position.X == -1 && Position.Y == -1)
				Move(Host.MousePosition);
		}

		private void Select(int num)
		{
			Selected.Raise(num);
			Host.ReleaseMouse();
		}

		private class Petal : Widget
		{
			private readonly TextLine textLine;
			private readonly int num;

			public Petal(Widget parent, string text, int num)
				: base(parent)
			{
				this.num = num;
				this.textLine = new TextLine(Fonts.Default);
				this.textLine.TextColor = Color.Yellow;
				this.textLine.Append(text);

				// TODO: text block size should be used here
				Resize(textLine.TextWidth + 14, Fonts.Default.Height + 8);
			}

			public int Num
			{
				get { return num; }
			}

			protected override void OnDraw(DrawingContext dc)
			{
				dc.Draw(back, 0, 0, Width, Height);
				dc.Draw(box, 0, 0, Width, Height);
				dc.Draw(textLine, 6, 4);
			}

			protected override void OnDispose()
			{
				textLine.Dispose();
			}
		}
	}
}
