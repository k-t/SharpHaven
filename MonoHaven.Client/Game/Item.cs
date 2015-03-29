using MonoHaven.Graphics;
using MonoHaven.Utils;

namespace MonoHaven.Game
{
	public class Item
	{
		private Delayed<Drawable> image;
		private Delayed<string> tooltip;

		public Item()
		{
			Amount = -1;
			Quality = -1;
			Meter = -1;
		}

		public Item(Delayed<ItemMold> mold) : this()
		{
			image = mold.Select(x => x.Image);
			tooltip = mold.Select(x => x.Tooltip);
		}

		public Drawable Image
		{
			get { return image != null ? image.Value : null; }
			set { image = new Delayed<Drawable>(value); }
		}

		public string Tooltip
		{
			get { return tooltip != null ? tooltip.Value : null; }
			set { tooltip = new Delayed<string>(value); }
		}

		public int Amount
		{
			get;
			set;
		}

		public int Quality
		{
			get;
			set;
		}

		public int Meter
		{
			get;
			set;
		}
	}
}
