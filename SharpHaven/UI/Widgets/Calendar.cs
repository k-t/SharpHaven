using System;
using SharpHaven.Client;
using SharpHaven.Graphics;
using SharpHaven.Utils;

namespace SharpHaven.UI.Widgets
{
	public class Calendar : Widget
	{
		private const double hbr = 23;

		private static readonly Drawable background;
		private static readonly Drawable nightSky;
		private static readonly Drawable nightScape;
		private static readonly Drawable daySky;
		private static readonly Drawable dayScape;
		private static readonly Drawable sun;
		private static readonly Drawable[] moons;

		static Calendar()
		{
			background = App.Resources.Get<Drawable>("gfx/hud/calendar/setting");
			daySky = App.Resources.Get<Drawable>("gfx/hud/calendar/daysky");
			dayScape = App.Resources.Get<Drawable>("gfx/hud/calendar/dayscape");
			nightSky = App.Resources.Get<Drawable>("gfx/hud/calendar/nightsky");
			nightScape = App.Resources.Get<Drawable>("gfx/hud/calendar/nightscape");
			sun = App.Resources.Get<Drawable>("gfx/hud/calendar/sun");

			moons = new Drawable[8];
			for (int i = 0; i < moons.Length; i++)
				moons[i] = App.Resources.Get<Drawable>(string.Format("gfx/hud/calendar/m{0:D2}", i));
		}

		public Calendar(Widget parent) : base(parent)
		{
			Size = background.Size;
		}

		public ClientSession Session
		{
			get;
			set;
		}

		protected override void OnDraw(DrawingContext dc)
		{
			if (Session == null)
				return;

			var astro = Session.Time;
			var mp = (int)(astro.MoonPhase * moons.Length);
			var moon = moons[mp];

			var mc = MathUtils.PolarToCartesian((astro.DayTime + 0.25) * 2 * Math.PI, hbr);
			var sc = MathUtils.PolarToCartesian((astro.DayTime + 0.75) * 2 * Math.PI, hbr);
			
			dc.Draw(background, 0, 0);
			dc.Draw(astro.IsNight ? nightSky : daySky, 0, 0);
			dc.Draw(moon, mc.X + (Width - moon.Width) / 2, mc.Y + (Height - moon.Height) / 2);
			dc.Draw(sun, sc.X + (Width - sun.Width) / 2, sc.Y + (Height - sun.Height) / 2);
			dc.Draw(astro.IsNight ? nightScape : dayScape, 0, 0);
		}
	}
}
