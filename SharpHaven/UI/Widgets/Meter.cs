using System.Collections.Generic;
using System.Linq;
using SharpHaven.Client;
using SharpHaven.Graphics;

namespace SharpHaven.UI.Widgets
{
	public class Meter : Widget
	{
		private static readonly Coord2D off = new Coord2D(13, 7);
		private static readonly Coord2D fsz = new Coord2D(63, 18);
		private static readonly Coord2D msz = new Coord2D(49, 4);

		private List<Metric> metrics;

		public Meter(Widget parent) : base(parent)
		{
			metrics = new List<Metric>();
			Size = fsz;
		}

		public Drawable Background
		{
			get;
			set;
		}

		public void SetMetrics(IEnumerable<Metric> metrics)
		{
			this.metrics = metrics.ToList();
		}

		protected override void OnDraw(DrawingContext dc)
		{
			if (Background == null)
				return;

			dc.SetColor(Color.FromArgb(255, 0, 0, 0));
			dc.DrawRectangle(off.X, off.Y, msz.X, msz.Y);
			foreach (var metric in metrics)
			{
				int w = msz.X;
				w = (w * metric.Value) / 100;
				dc.SetColor(metric.Color);
				dc.DrawRectangle(off.X , off.Y, w, msz.Y);
			}
			dc.ResetColor();
			dc.Draw(Background, 0, 0);
		}
	}
}
