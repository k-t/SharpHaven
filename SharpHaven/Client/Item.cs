using System;
using Haven;
using Haven.Utils;
using SharpHaven.Graphics;

namespace SharpHaven.Client
{
	public class Item
	{
		private Delayed<Drawable> image;
		private Delayed<string> tooltip;
		private int amount;
		private int meter;
		private int quality;

		public Item()
		{
			Amount = -1;
			Quality = -1;
			Meter = -1;
		}

		public event Action Changed;

		public Item(Delayed<ItemProto> mold) : this()
		{
			image = mold.Select(x => x.Image);
			tooltip = mold.Select(x => x.Tooltip);
		}

		public Drawable Image
		{
			get { return image?.Value; }
			set { image = new Delayed<Drawable>(value); }
		}

		public string Tooltip
		{
			get { return tooltip?.Value; }
			set
			{
				tooltip = new Delayed<string>(value);
				Changed.Raise();
			}
		}

		public int Amount
		{
			get { return amount; }
			set
			{
				amount = value;
				Changed.Raise();
			}
		}

		public int Quality
		{
			get { return quality; }
			set
			{
				quality = value;
				Changed.Raise();
			}
		}

		public int Meter
		{
			get { return meter; }
			set
			{
				meter = value;
				Changed.Raise();
			}
		}
	}
}
