using System;
using System.Collections.Generic;
using System.Drawing;
using OpenTK.Input;
using SharpHaven.Graphics;
using SharpHaven.Graphics.Text;
using SharpHaven.Input;

namespace SharpHaven.UI.Widgets
{
	public class FlowerMenu : Widget
	{
		private static readonly Drawable box;
		private static readonly Drawable back;

		static FlowerMenu()
		{
			back = App.Resources.Get<Drawable>("gfx/hud/bgtex");
			box = App.Resources.Get<Drawable>("custom/gfx/hud/invsq");
		}

		private readonly int optionCount;

		public FlowerMenu(Widget parent, IEnumerable<string> options) : base(parent)
		{
			int n = 0;
			int y = 0;
			int menuWidth = 0;

			foreach (var option in options)
			{
				var item = new MenuItem(this, option, n++);
				item.Move(0, y);
				y += item.Height - 1;
				menuWidth = Math.Max(menuWidth, item.Width);
			}
			// make same width
			foreach (var widget in Children)
				widget.Resize(menuWidth, widget.Height);

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
				var petal = GetChildAt(e.Position) as MenuItem;
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

		private void Select(int num)
		{
			Selected.Raise(num);
			Host.ReleaseMouse();
		}

		private class MenuItem : Widget
		{
			private readonly TextLine text;
			private readonly TextLine numText;
			private readonly int num;

			public MenuItem(Widget parent, string text, int num)
				: base(parent)
			{
				this.num = num;

				this.text = new TextLine(Fonts.Default);
				this.text.Append(text);
				this.text.TextColor = Color.White;

				this.numText = new TextLine(Fonts.Default);
				this.numText.AppendFormat("{0}", num + 1);
				this.numText.TextColor = Color.Yellow;
				this.numText.TextAlign = TextAlign.Right;

				int width = Math.Max(40, this.text.TextWidth + this.numText.TextWidth + 15);
				Resize(width, Fonts.Default.Height + 4);
			}

			public int Num
			{
				get { return num; }
			}

			protected override void OnDraw(DrawingContext dc)
			{
				dc.Draw(back, 0, 0, Width, Height);
				dc.Draw(box, 0, 0, Width, Height);
				dc.Draw(text, 4, 2, Width - 8, Height - 4);
				dc.Draw(numText, 4, 2, Width - 8, Height - 4);
			}

			protected override void OnDispose()
			{
				text.Dispose();
				numText.Dispose();
			}
		}
	}
}
