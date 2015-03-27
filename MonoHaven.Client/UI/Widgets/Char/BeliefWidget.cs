using System;
using MonoHaven.Graphics;

namespace MonoHaven.UI.Widgets
{
	public class BeliefWidget : Widget
	{
		private static readonly Drawable slider;
		private static readonly Drawable flarp;
		private static readonly Drawable lbu;
		private static readonly Drawable lbd;
		private static readonly Drawable lbg;
		private static readonly Drawable rbu;
		private static readonly Drawable rbd;
		private static readonly Drawable rbg;

		static BeliefWidget()
		{
			slider = App.Resources.Get<Drawable>("gfx/hud/charsh/bslider");
			flarp = App.Resources.Get<Drawable>("gfx/hud/sflarp");
			lbu = App.Resources.Get<Drawable>("gfx/hud/charsh/leftup");
			lbd = App.Resources.Get<Drawable>("gfx/hud/charsh/leftdown");
			lbg = App.Resources.Get<Drawable>("gfx/hud/charsh/leftgrey");
			rbu = App.Resources.Get<Drawable>("gfx/hud/charsh/rightup");
			rbd = App.Resources.Get<Drawable>("gfx/hud/charsh/rightdown");
			rbg = App.Resources.Get<Drawable>("gfx/hud/charsh/rightgrey");
		}

		private readonly ImageButton btnLeft;
		private readonly ImageButton btnRight;
		private readonly Image imgFlarper;
		private bool enabled = true;

		public BeliefWidget(Widget parent, string left, string right) : base(parent)
		{
			Resize(140, 20);

			var imgLeft = new Image(this);
			imgLeft.Drawable = App.Resources.Get<Drawable>("gfx/hud/charsh/" + left);

			var imgSlider = new Image(this);
			imgSlider.Drawable = slider;
			imgSlider.Move(32, 4);

			var imgRight = new Image(this);
			imgRight.Move(128, 0);
			imgRight.Drawable = App.Resources.Get<Drawable>("gfx/hud/charsh/" + right);
		
			imgFlarper = new Image(this);
			imgFlarper.Move(0, 2);
			imgFlarper.Drawable = flarp;

			btnLeft = new ImageButton(this);
			btnLeft.Move(16, 0);
			btnLeft.Click += () => SliderMoved.Raise(-1);

			btnRight = new ImageButton(this);
			btnRight.Move(112, 0);
			btnRight.Click += () => SliderMoved.Raise(1);

			UpdateButtons();
		}

		public event Action<int> SliderMoved;

		public bool Enabled
		{
			get { return enabled; }
			set
			{
				enabled = value;
				UpdateButtons();
			}
		}

		private void UpdateButtons()
		{
			if (Enabled)
			{
				btnLeft.Image = lbu;
				btnLeft.PressedImage = lbd;
				btnRight.Image = rbu;
				btnRight.PressedImage = rbd;
			}
			else
			{
				btnLeft.Image = lbg;
				btnLeft.PressedImage = lbg;
				btnRight.Image = rbg;
				btnRight.PressedImage = rbg;
			}
		}

		public void SetSliderPosition(int value)
		{
			imgFlarper.Move((7 * (value + 5)) + 31, imgFlarper.Y);
		}
	}
}
