using System;
using System.Linq;
using MonoHaven.Graphics;

namespace MonoHaven.UI
{
	public class HudMenu : Widget
	{
		private const int ButtonHide = 0;
		private const int ButtonInv = 1;
		private const int ButtonEquip = 2;
		private const int ButtonChara = 3;
		private const int ButtonBuddies = 4;
		private const int ButtonOptions = 5;
		private const int ButtonCount = 6;

		private static readonly Drawable background;
		private static readonly Drawable[] buttonImages;

		static HudMenu()
		{
			background = App.Resources.GetTexture("gfx/hud/slen/mcircle");
			buttonImages = new[] {
				"gfx/hud/slen/hbu", "gfx/hud/slen/hbd",
				"gfx/hud/slen/invu", "gfx/hud/slen/invd",
				"gfx/hud/slen/equu", "gfx/hud/slen/equd",
				"gfx/hud/slen/chru", "gfx/hud/slen/chrd",
				"gfx/hud/slen/budu", "gfx/hud/slen/budd",
				"gfx/hud/slen/optu", "gfx/hud/slen/optd" }
				.Select(x => App.Resources.GetTexture(x))
				.ToArray();
		}

		public HudMenu(Widget parent) : base(parent)
		{
			base.SetSize(background.Width, background.Height);

			for (int i = 0; i < ButtonCount * 2; i += 2)
			{
				var button = new ImageButton(this)
				{
					Image = buttonImages[i],
					PressedImage = buttonImages[i + 1]
				};
				button.SetSize(buttonImages[i].Width, buttonImages[i].Height);
				int buttonIndex = i; // catch to closure
				button.Clicked += () => ButtonClicked.Raise(buttonIndex);
			}
		}

		public event Action<int> ButtonClicked;

		protected override void OnDraw(DrawingContext dc)
		{
			dc.Draw(background, 0, 0);
		}
	}
}
