using Haven.Utils;
using SharpHaven.Graphics;

namespace SharpHaven.Client
{
	public class Buff
	{
		private Delayed<Drawable> image;
		private Delayed<string> tooltip;

		public Buff(int id, Delayed<BuffProto> mold)
		{
			Id = id;
			this.image = mold.Select(x => x.Image);
			this.tooltip = mold.Select(x => x.Tooltip);
		}

		public int Id { get; }

		public bool IsMajor { get; set; }

		public Drawable Image
		{
			get { return image?.Value; }
			set { image = new Delayed<Drawable>(value); }
		}

		public string Tooltip
		{
			get { return tooltip?.Value; }
			set { tooltip = new Delayed<string>(value); }
		}

		public int Amount
		{
			get;
			set;
		}
	}
}
