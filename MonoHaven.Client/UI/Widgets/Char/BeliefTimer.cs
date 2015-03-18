using MonoHaven.Graphics;
using System;

namespace MonoHaven.UI.Widgets
{
	public class BeliefTimer : Widget
	{
		private static readonly Drawable onImage;
		private static readonly Drawable offImage;

		static BeliefTimer()
		{
			onImage = App.Resources.GetImage("gfx/hud/charsh/shield");
			offImage = App.Resources.GetImage("gfx/hud/charsh/shieldgray");
		}

		private int time;

		public BeliefTimer(Widget parent) : base(parent)
		{
			Resize(offImage.Size);
		}

		public event Action TimeChanged;

		public int Time
		{
			get { return time; }
			set
			{
				time = value;
				TimeChanged.Raise();
			}
		}

		protected override void OnDraw(DrawingContext dc)
		{
			var image = Time > 0 ? offImage : onImage;
			dc.Draw(image, 0, 0);
		}
	}
}
