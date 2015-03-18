using MonoHaven.Graphics;

namespace MonoHaven.UI.Widgets
{
	public class ISBox : Widget
	{
		private static readonly Drawable background;

		static ISBox()
		{
			background = App.Resources.GetImage("gfx/hud/bosq");
		}

		private int remaining;
		private int available;
		private int built;
		private readonly Label label;

		public ISBox(Widget parent)
			: base(parent)
		{
			Resize(background.Size);

			label = new Label(this, Fonts.Heading);
			label.Move(40, (Height - label.Height) / 2);
			UpdateLabel();
		}

		public Drawable Image
		{
			get;
			set;
		}

		public int Remaining
		{
			get { return remaining; }
			set
			{
				remaining = value;
				UpdateLabel();
			}
		}

		public int Available
		{
			get { return available; }
			set
			{
				available = value;
				UpdateLabel();
			}
		}

		public int Built
		{
			get { return built; }
			set
			{
				built = value;
				UpdateLabel();
			}
		}

		protected override void OnDraw(DrawingContext dc)
		{
			dc.Draw(background, 0, 0);
			if (Image != null)
				dc.Draw(Image, 6, (Height - Image.Height) / 2);
		}

		private void UpdateLabel()
		{
			label.Text = string.Format("{0}/{1}/{2}", Remaining, Available, Built);
		}
	}
}
