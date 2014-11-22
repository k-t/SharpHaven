#region License
// Copyright (c) 2014 Marat Vildanov <marat.vildanov@gmail.com>
// Distributed under the MIT License.
// (See accompanying LICENSE file or copy at http://opensource.org/licenses/MIT)
#endregion

using System;
using MonoHaven.Game;
using MonoHaven.Graphics;
using MonoHaven.Utils;
using OpenTK;

namespace MonoHaven.UI
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
			var atlas = new TextureAtlas(512, 512);

			background = atlas.Add(App.Instance.Resources.GetImage("gfx/hud/calendar/setting").Data);
			daySky = atlas.Add(App.Instance.Resources.GetImage("gfx/hud/calendar/daysky").Data);
			dayScape = atlas.Add(App.Instance.Resources.GetImage("gfx/hud/calendar/dayscape").Data);
			nightSky = atlas.Add(App.Instance.Resources.GetImage("gfx/hud/calendar/nightsky").Data);
			nightScape = atlas.Add(App.Instance.Resources.GetImage("gfx/hud/calendar/nightscape").Data);
			sun = atlas.Add(App.Instance.Resources.GetImage("gfx/hud/calendar/sun").Data);

			moons = new Drawable[8];
			for (int i = 0; i < moons.Length; i++)
				moons[i] = atlas.Add(App.Instance.Resources.GetImage(string.Format("gfx/hud/calendar/m{0:D2}", i)).Data);
		}

		private readonly GameTime time;

		public Calendar(Widget parent, GameTime time) : base(parent)
		{
			this.time = time;
			base.SetSize(background.Width, background.Height);
		}

		protected override void OnDraw(DrawingContext dc)
		{
			var mp = (int)(time.MoonPhase * moons.Length);
			var moon = moons[mp];

			var mc = MathUtils.PolarToCartesian((time.DayTime + 0.25) * 2 * Math.PI, hbr);
			var sc = MathUtils.PolarToCartesian((time.DayTime + 0.75) * 2 * Math.PI, hbr);
			
			dc.Draw(background, 0, 0);
			dc.Draw(time.IsNight ? nightSky : daySky, 0, 0);
			dc.Draw(moon, mc.X + (Width - moon.Width) / 2, mc.Y + (Height - moon.Height) / 2);
			dc.Draw(sun, sc.X + (Width - sun.Width) / 2, sc.Y + (Height - sun.Height) / 2);
			dc.Draw(time.IsNight ? nightScape : dayScape, 0, 0);
		}
	}
}
