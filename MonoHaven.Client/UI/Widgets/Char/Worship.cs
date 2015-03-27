using System;
using MonoHaven.Game;
using MonoHaven.Graphics;

namespace MonoHaven.UI.Widgets
{
	public class Worship : Widget
	{
		private static readonly Drawable ancestors;
		private static readonly Drawable nmeter;

		static Worship()
		{
			ancestors = App.Resources.Get<Drawable>("gfx/hud/charsh/ancestors");
			nmeter = App.Resources.Get<Drawable>("gfx/hud/charsh/numenmeter");
		}

		private readonly ItemWidget[] wishes = new ItemWidget[3];
		private readonly Label lblTitle;
		private readonly Label lblNumen;

		public Worship(Widget parent) : base(parent)
		{
			lblTitle = new Label(this, Fonts.LabelText);
			lblTitle.TextAlign = TextAlign.Center;

			for (int i = 0; i < wishes.Length; i++)
			{
				var inv = new InventoryWidget(this);
				inv.Move(3 + i * 31, 122);
				inv.SetInventorySize(1, 1);
				wishes[i] = new ItemWidget(inv, null);
				wishes[i].Move(1, 1);
				wishes[i].Visible = false;
			}

			var btnForfeit = new Button(this, 80);
			btnForfeit.Text = "Forfeit";
			btnForfeit.Move(10, 160);
			btnForfeit.Click += () => Forfeit.Raise();

			lblNumen = new Label(this, Fonts.LabelText);
			lblNumen.Text = "0";

			Resize(100, 200);
		}

		public event Action Forfeit;

		public string Title
		{
			get { return lblTitle.Text; }
			set { lblTitle.Text = value; }
		}

		public void SetNumenCount(int numens)
		{
			lblNumen.Text = numens.ToString();
		}

		public void SetWish(int index, Item item)
		{
			wishes[index].Item = item;
			wishes[index].Visible = true;
		}

		protected override void OnDraw(DrawingContext dc)
		{
			dc.Draw(ancestors, (Width - ancestors.Width) / 2, 15);
			dc.Draw(nmeter, (Width - nmeter.Width) / 2, 100);
		}

		protected override void OnSizeChanged()
		{
			lblTitle.Resize(Width, lblTitle.Height);
			lblNumen.Move((Width - nmeter.Width) / 2 + 18, 100 + 21 - nmeter.Height);
		}
	}
}
