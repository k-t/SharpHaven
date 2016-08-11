using System;
using Haven;
using SharpHaven.Graphics;

namespace SharpHaven.UI.Widgets
{
	public class BeliefTimer : Widget
	{
		private static readonly Drawable onImage;
		private static readonly Drawable offImage;

		static BeliefTimer()
		{
			onImage = App.Resources.Get<Drawable>("gfx/hud/charsh/shield");
			offImage = App.Resources.Get<Drawable>("gfx/hud/charsh/shieldgray");
		}

		private int time;

		public BeliefTimer(Widget parent) : base(parent)
		{
			Size = offImage.Size;
		}

		public event Action TimeChanged;

		public int Time
		{
			get { return time; }
			set
			{
				time = value;
				Tooltip = GetTooltip();
				TimeChanged.Raise();
			}
		}

		protected override void OnDraw(DrawingContext dc)
		{
			var image = Time > 0 ? offImage : onImage;
			dc.Draw(image, 0, 0);
		}

		private Tooltip GetTooltip()
		{
			if (Time == 0)
				return null;
			if (Time < 3600)
				return new Tooltip("Less than one hour left");
			return new Tooltip(string.Format("{0} hours left", ((Time - 1) / 3600) + 1));
		}
	}
}
