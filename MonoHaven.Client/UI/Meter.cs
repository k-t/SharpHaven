using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using MonoHaven.Game;
using MonoHaven.Graphics;

namespace MonoHaven.UI
{
	public class Meter : Widget
	{
		private static readonly Point off = new Point(13, 7);
		private static readonly Size fsz = new Size(63, 18);
		private static readonly Point msz = new Point(49, 4);

		private List<Metric> metrics;

		public Meter(Widget parent) : base(parent)
		{
			metrics = new List<Metric>();
			Resize(fsz);
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
