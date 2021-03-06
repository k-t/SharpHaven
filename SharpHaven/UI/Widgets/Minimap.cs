﻿using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Haven;
using Haven.Utils;
using NLog;
using SharpHaven.Client;
using SharpHaven.Graphics;
using SharpHaven.Utils;

namespace SharpHaven.UI.Widgets
{
	public class Minimap : Widget
	{
		private static readonly Logger Log = LogManager.GetCurrentClassLogger();

		private static readonly Drawable bg;
		private static readonly Drawable nomap;
		private static readonly Drawable mark;

		static Minimap()
		{
			bg = App.Resources.Get<Drawable>("gfx/hud/mmap/ptex");
			nomap = App.Resources.Get<Drawable>("gfx/hud/mmap/nomap");
			mark = App.Resources.Get<Drawable>("gfx/hud/mmap/x");
		}

		private readonly ClientSession session;
		private readonly MinimapProvider provider;
		private readonly ConcurrentDictionary<string, Drawable> cache;

		public Minimap(Widget parent, ClientSession session) : base(parent)
		{
			this.session = session;
			this.cache = new ConcurrentDictionary<string, Drawable>();
			this.provider = new MinimapProvider(App.Config.MapUrl);
		}

		protected override void OnDraw(DrawingContext dc)
		{
			bool missing = false;

			dc.SetClip(0, 0, Width, Height);
			dc.Draw(bg, 0, 0, Width, Height);

			var tc = Geometry.MapToTile(session.WorldPosition);

			int bx = (tc.X - Width / 2).Div(Geometry.GridWidth);
			int by = (tc.Y - Height / 2).Div(Geometry.GridHeight);
			int ex = (tc.X + Width / 2).Div(Geometry.GridWidth);
			int ey = (tc.Y + Height / 2).Div(Geometry.GridHeight);
			for (int gx = bx; gx <= ex; gx++)
				for (int gy = by; gy <= ey; gy++)
				{
					var grid = session.Map.GetGrid(gx, gy);
					if (grid == null)
						continue;

					if (string.IsNullOrEmpty(grid.MinimapName))
					{
						missing = true;
						break;
					}

					var image = GetMinimap(grid);
					if (image != null)
					{
						var tx = gx * Geometry.GridWidth;
						var ty = gy * Geometry.GridHeight;
						dc.Draw(image, tx - tc.X + Width / 2, ty - tc.Y + Height / 2);
					}
				}

			if (!missing)
			{
				foreach (var member in session.Party.Members)
				{
					if (!member.Location.HasValue)
						continue;
					var p = Geometry.MapToTile(member.Location.Value).Sub(tc);
					int x = p.X + Width / 2;
					int y = p.Y + Height / 2;
					dc.SetColor(Color.FromArgb(128, member.Color));
					dc.Draw(mark, x - mark.Width / 2, y - mark.Height / 2);
					dc.ResetColor();
				}
			}

			dc.ResetClip();
		}

		private Drawable GetMinimap(MapGrid grid)
		{
			Drawable image;
			if (cache.TryGetValue(grid.MinimapName, out image))
				return image;

			cache[grid.MinimapName] = null;
			Task.Factory.StartNew(() => RequestMinimap(grid));
			return null;
		}

		private void RequestMinimap(MapGrid grid)
		{
			try
			{
				var bytes = provider.Get(grid.MinimapName);
				// texture needs to be loaded on the main thread
				App.QueueOnMainThread(() =>
				{
					var tex = TextureSlice.FromBitmap(bytes);
					var image = new Picture(tex, null);
					cache[grid.MinimapName] = image;
				});
			}
			catch (Exception ex)
			{
				Log.Error(ex, "Couldn't get minimap at " + grid.Coord);
			}
		}
	}
}
