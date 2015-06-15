using SharpHaven.Graphics;
using SharpHaven.Utils;

namespace SharpHaven.Client
{
	public class Buff
	{
		private readonly int id;
		private Delayed<Drawable> image;
		private Delayed<string> tooltip;

		public Buff(int id, Delayed<BuffProto> mold)
		{
			this.id = id;
			this.image = mold.Select(x => x.Image);
			this.tooltip = mold.Select(x => x.Tooltip);
		}

		public int Id
		{
			get { return id; }
		}

		public bool IsMajor
		{
			get;
			set;
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
	}
}
