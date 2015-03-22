using MonoHaven.Graphics;
using MonoHaven.Utils;

namespace MonoHaven.Game
{
	public class Item
	{
		private Delayed<ItemMold> mold;
		private Drawable image;
		private string tooltip;

		public Item()
		{
			Num = -1;
			Quality = -1;
		}

		public Item(ItemMold mold) : this()
		{
			Image = mold.Image;
			Tooltip = mold.Tooltip;
		}

		public Item(Delayed<ItemMold> mold) : this()
		{
			this.mold = mold;
		}

		public Drawable Image
		{
			get
			{
				if (image != null) return image;
				if (mold != null && mold.Value != null) return mold.Value.Image;
				return null;
			}
			set { image = value; }
		}

		public string Tooltip
		{
			get
			{
				if (tooltip != null) return tooltip;
				if (mold != null && mold.Value != null) return mold.Value.Tooltip;
				return null;
			}
			set { tooltip = value; }
		}

		public int Num
		{
			get;
			set;
		}

		public int Quality
		{
			get;
			set;
		}
	}
}
