using System;
using System.Linq;
using MonoHaven.Graphics;

namespace MonoHaven.UI.Widgets
{
	public class HudMenu : Widget
	{
		private static readonly Drawable background;
		private static readonly Drawable[] buttonImages;
		private static readonly int buttonCount;

		static HudMenu()
		{
			background = App.Resources.Get<Drawable>("gfx/hud/invsq");
			buttonImages = new[] {
				"custom/gfx/hud/slen/invu", "custom/gfx/hud/slen/invd",
				"custom/gfx/hud/slen/equu", "custom/gfx/hud/slen/equd",
				"custom/gfx/hud/slen/chru", "custom/gfx/hud/slen/chrd",
				"custom/gfx/hud/slen/budu", "custom/gfx/hud/slen/budd",
				"custom/gfx/hud/slen/optu", "custom/gfx/hud/slen/optd" }
				.Select(x => App.Resources.Get<Drawable>(x))
				.ToArray();
			buttonCount = buttonImages.Length / 2;
		}

		public HudMenu(Widget parent) : base(parent)
		{
			Resize((background.Width - 1) * buttonCount, background.Height - 1);

			int x = 0;
			for (int i = 0; i < buttonCount; i++)
			{
				var btn = new ImageButton(this)
				{
					Image = buttonImages[i * 2],
					PressedImage = buttonImages[i * 2 + 1]
				};
				btn.Resize(buttonImages[i * 2].Size);
				btn.Move(x + 1, 1);
				x += background.Width - 1;

				var button = (Button)i;
				btn.Click += () => ButtonClick.Raise(button);
			}
		}

		protected override void OnDraw(DrawingContext dc)
		{
			int x = 0;
			for (int i = 0; i < buttonCount; i++)
			{
				dc.Draw(background, x, 0);
				x += background.Width - 1;
			}
		}

		public event Action<Button> ButtonClick;

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
