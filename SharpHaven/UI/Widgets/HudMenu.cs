﻿using System;
using System.Linq;
using Haven;
using SharpHaven.Graphics;

namespace SharpHaven.UI.Widgets
{
	public class HudMenu : Widget
	{
		private static readonly Drawable background;
		private static readonly Drawable[] images;
		private static readonly string[] tooltips;
		private static readonly int buttonCount;

		static HudMenu()
		{
			background = App.Resources.Get<Drawable>("gfx/hud/invsq");
			images = new[] {
				"gfx/hud/slen/invu", "gfx/hud/slen/invd",
				"gfx/hud/slen/equu", "gfx/hud/slen/equd",
				"gfx/hud/slen/chru", "gfx/hud/slen/chrd",
				"gfx/hud/slen/budu", "gfx/hud/slen/budd",
				"gfx/hud/slen/optu", "gfx/hud/slen/optd" }
				.Select(x => App.Resources.Get<Drawable>(x))
				.ToArray();
			tooltips = new[] {
				"Inventory",
				"Equipment",
				"Character",
				"Kin",
				"Options"
			};
			buttonCount = images.Length / 2;
		}

		public HudMenu(Widget parent) : base(parent)
		{
			this.Resize(background.Width, (background.Height - 1) * buttonCount);

			int y = 0;
			for (int i = 0; i < buttonCount; i++)
			{
				var btn = new ImageButton(this)
				{
					Image = images[i * 2],
					PressedImage = images[i * 2 + 1]
				};
				btn.Resize(images[i * 2].Size);
				btn.Move(1, y + 1);
				btn.Tooltip = new Tooltip(tooltips[i]);
				y += background.Height - 1;

				var button = (Button)i;
				btn.Click += () => ButtonClick.Raise(button);
			}
		}

		public event Action<Button> ButtonClick;

		public void Activate(Button button)
		{
			ButtonClick.Raise(button);
		}

		protected override void OnDraw(DrawingContext dc)
		{
			int y = 0;
			for (int i = 0; i < buttonCount; i++)
			{
				dc.Draw(background, 0, y);
				y += background.Height - 1;
			}
		}

		public enum Button
		{
			Inventory = 0,
			Equipment = 1,
			Character = 2,
			BuddyList = 3,
			Options = 4
		}
	}
}
