﻿using System;
using System.Collections.Generic;
using System.Drawing;
using MonoHaven.Graphics;
using MonoHaven.Input;

namespace MonoHaven.UI
{
	public class FlowerMenu : Widget
	{
		private static readonly Drawable box;
		private static readonly Drawable back;

		static FlowerMenu()
		{
			back = App.Resources.GetImage("gfx/hud/bgtex");
			box = App.Resources.GetImage("custom/ui/wbox");
		}

		public FlowerMenu(Widget parent, IEnumerable<string> options) : base(parent)
		{
			int n = 1;
			foreach (var option in options)
			{
				var petal = new Petal(this, option, n++);
				petal.Move(0, (n - 1) * 25);
			}
			IsFocusable = true;
			Host.GrabMouse(this);
			Host.RequestKeyboardFocus(this);
		}

		public event Action<int> Selected;

		protected override void OnMouseButtonDown(MouseButtonEvent e)
		{
			Selected.Raise(-1);
			Host.ReleaseMouse();
		}

		private class Petal : Widget
		{
			private readonly int num;
			private readonly string text;
			private readonly TextBlock textBlock;

			public Petal(Widget parent, string text, int num)
				: base(parent)
			{
				this.num = num;
				this.text = text;
				this.textBlock = new TextBlock(Fonts.Default);
				this.textBlock.TextColor = Color.Yellow;
				this.textBlock.Append(text);

				// TODO: text block size should be used here
				Resize(textBlock.TextWidth + 14, Fonts.Default.Height + 8);
			}

			protected override void OnDraw(DrawingContext dc)
			{
				dc.Draw(back, 0, 0, Width, Height);
				dc.Draw(box, 0, 0, Width, Height);
				dc.Draw(textBlock, 6, 4);
			}

			protected override void OnDispose()
			{
				textBlock.Dispose();
			}
		}
	}
}
