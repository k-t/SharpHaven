using System;
using System.Linq;
using MonoHaven.Graphics;

namespace MonoHaven.UI.Widgets
{
	public class HudMenu : Widget
	{
		private static readonly Drawable background;
		private static readonly Drawable[] buttonImages;

		static HudMenu()
		{
			background = App.Resources.Get<Drawable>("gfx/hud/slen/mcircle");
			buttonImages = new[] {
				"gfx/hud/slen/hbu", "gfx/hud/slen/hbd",
				"gfx/hud/slen/invu", "gfx/hud/slen/invd",
				"gfx/hud/slen/equu", "gfx/hud/slen/equd",
				"gfx/hud/slen/chru", "gfx/hud/slen/chrd",
				"gfx/hud/slen/budu", "gfx/hud/slen/budd",
				"gfx/hud/slen/optu", "gfx/hud/slen/optd" }
				.Select(x => App.Resources.Get<Drawable>(x))
				.ToArray();
		}

		public HudMenu(Widget parent) : base(parent)
		{
			Resize(background.Size);

			for (int i = 0; i < 6; i++)
			{
				var buttonWidget = new ImageButton(this)
				{
					Image = buttonImages[i * 2],
					PressedImage = buttonImages[i * 2 + 1]
				};
				buttonWidget.Resize(buttonImages[i * 2].Size);
				var button = (Button)i;
				buttonWidget.Clicked += () => ButtonClicked.Raise(button);
			}
		}

		public event Action<Button> ButtonClicked;

		protected override void OnDraw(DrawingContext dc)
		{
			dc.Draw(background, 0, 0);
		}

		public enum Button
		{
			Hide = 0,
			Inventory = 1,
			Equipment = 2,
			Character = 3,
			BuddyList = 4,
			Options = 5
		}
	}
}
