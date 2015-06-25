﻿using SharpHaven.Graphics;

namespace SharpHaven.Client
{
	public class BuffProto
	{
		private readonly Drawable image;
		private readonly string tooltip;

		public BuffProto(Drawable image, string tooltip)
		{
			this.image = image;
			this.tooltip = tooltip;
		}

		public Drawable Image
		{
			get { return image; }
		}

		public string Tooltip
		{
			get { return tooltip; }
		}
	}
}