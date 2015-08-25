using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using SharpHaven.Graphics;

namespace SharpHaven.UI.Widgets
{
	public class FoodMeter : Widget
	{
		private static readonly Drawable background;

		static FoodMeter()
		{
			background = App.Resources.Get<Drawable>("gfx/hud/charsh/foodm");
		}

		private int cap;
		private readonly List<El> els = new List<El>();

		public FoodMeter(Widget parent) : base(parent)
		{
			els = new List<El>();
			Size = background.Size;
		}

		public void Update(object[] args)
		{
			cap = (int)args[0];

			els.Clear();
			for(int i = 1; i < args.Length; i += 3)
			{
				var id = (string)args[i];
				var amount = (int)args[i + 1];
				var color = (Color)args[i + 2];
				els.Add(new El(id, amount, color));
			}

			UpdateTooltip();
		}

		protected override void OnDraw(DrawingContext dc)
		{
			dc.SetColor(Color.Black);
			dc.DrawRectangle(4, 4, Width - 8, Height - 8);
			dc.SetColor(255, 255, 255, 128);
			dc.Draw(background, 0, 0);
			int x = 4;
			foreach (var el in els)
			{
				int w = (174 * el.Amount) / cap;
				dc.SetColor(el.Color);
				dc.DrawRectangle(x, 4, w, 24);
				x += w;
			}
			dc.SetColor(255, 255, 255, 128);
			dc.Draw(background, 0, 0);
			dc.ResetColor();
		}

		private void UpdateTooltip()
		{
			if (els.Count == 0)
				Tooltip = new Tooltip(string.Format("0 of {0:0.0}", cap / 10.0));
			else
			{
				var parts = els.Select(el => string.Format("{0:0.0} {1}", el.Amount / 10.0, el.Id));
				var sum = els.Sum(el => el.Amount);
				Tooltip = new Tooltip(string.Format("({0}) = {1:0.0} of {2:0.0}",
					string.Join(" + ", parts), sum / 10.0, cap / 10.0));
			}
		}

		private class El
		{
			public El(string id, int amount, Color color)
			{
				Id = id;
				Amount = amount;
				Color = color;
			}

			public string Id { get; set; }
			public int Amount { get; set; }
			public Color Color { get; set; }
		}
	}
}
