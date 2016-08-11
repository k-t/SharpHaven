using System;
using Haven;
using OpenTK;
using SharpHaven.Graphics;
using SharpHaven.Input;

namespace SharpHaven.UI.Widgets
{
	public class Speedget : Widget
	{
		private static readonly Drawable[,] images;
		private static readonly Point2D size;

		static Speedget()
		{
			images = new Drawable[4, 3];
			var names = new[] { "crawl", "walk", "run", "sprint" };
			var vars = new[] { "dis", "off", "on" };
			int w = 0;
			for (int i = 0; i < images.GetLength(0); i++)
			{
				for (int j = 0; j < images.GetLength(1); j++)
				{
					var image = App.Resources.Get<Drawable>("gfx/hud/meter/rmeter/" + names[i] + "-" + vars[j]);
					images[i, j] = image;
				}
				w += images[i, 0].Width;
			}
			size = new Point2D(w, images[0, 0].Height);
		}

		public Speedget(Widget parent) : base(parent)
		{
			Size = size;
		}

		public event Action<int> SpeedSelected;

		public int CurrentSpeed
		{
			get;
			set;
		}

		public int MaxSpeed
		{
			get;
			set;
		}

		protected override void OnDraw(DrawingContext dc)
		{
			int x = 0;
			for (int i = 0; i < 4; i++)
			{
				Drawable image;
				if (i == CurrentSpeed)
					image = images[i, 2];
				else if (i > MaxSpeed)
					image = images[i, 0];
				else
					image = images[i, 1];
				dc.Draw(image, x, 0);
				x += image.Width;
			}
		}

		protected override void OnMouseButtonDown(MouseButtonEvent e)
		{
			var p = MapFromScreen(e.Position);
			int x = 0;
			for (int i = 0; i < 4; i++)
			{
				x += images[i, 0].Width;
				if (p.X < x)
				{
					SpeedSelected.Raise(i);
					break;
				}
			}
			e.Handled = true;
		}

		protected override void OnMouseWheel(MouseWheelEvent e)
		{
			int speed = MathHelper.Clamp(CurrentSpeed + e.Delta, 0, MaxSpeed);
			SpeedSelected.Raise(speed);
			e.Handled = true;
		}
	}
}
