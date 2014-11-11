﻿using System;
using System.Drawing;
using MonoHaven.Graphics;
using MonoHaven.Utils;
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
			bl = App.Instance.Resources.GetTexture("gfx/hud/buttons/tbtn/left");
			br = App.Instance.Resources.GetTexture("gfx/hud/buttons/tbtn/right");
			bt = App.Instance.Resources.GetTexture("gfx/hud/buttons/tbtn/top");
			bb = App.Instance.Resources.GetTexture("gfx/hud/buttons/tbtn/bottom");
			dt = App.Instance.Resources.GetTexture("gfx/hud/buttons/tbtn/dtex");
			ut = App.Instance.Resources.GetTexture("gfx/hud/buttons/tbtn/utex");
		}

		public Button(Widget parent, int width) : base(parent)
		{
			SetSize(width, ButtonHeight);

			textBlock = new TextBlock(Fonts.Default);
			textBlock.TextColor = Color.Yellow;
			textBlock.TextAlign = TextAlign.Center;
			textBlock.Width = width;
		}

		public event EventHandler Pressed;

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

		protected override void OnDraw(DrawingContext dc)
		{
			// draw borders
			dc.Draw(isPressed ? dt : ut, 3, 3, Width - 6, 13);
			dc.Draw(bl, 0, 0);
			dc.Draw(br, Width - br.Width, 0);
			dc.Draw(bt, 3, 0, Width - 6, bt.Height);
			dc.Draw(bb, 3, Height - bb.Height, Width - 6, bb.Height);
			// draw content
			if (Image != null)
			{
				var cp = new Point((Width - Image.Width) / 2, (Height - Image.Height) / 2);
				if (isPressed) cp.Offset(1, 1);
				dc.Draw(Image, cp);
			}
			dc.Draw(textBlock, 0, 0);
		}

		protected override void OnMouseButtonDown(MouseButtonEventArgs e)
		{
			if (e.Button != MouseButton.Left)
				return;
			Host.GrabMouse(this);
			isPressed = true;
		}

		protected override void OnMouseButtonUp(MouseButtonEventArgs e)
		{
			if (e.Button != MouseButton.Left)
				return;
			Host.ReleaseMouse();
			isPressed = false;
			Pressed.Raise(this, EventArgs.Empty);
		}
	}
}
