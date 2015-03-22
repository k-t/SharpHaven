﻿using MonoHaven.Graphics;
using MonoHaven.Resources;

namespace MonoHaven.Game
{
	public class GameActionInfo
	{
		private readonly string name;
		private readonly ResourceRef parent;
		private readonly string tooltip;
		private readonly Drawable image;
		private readonly string[] verbs;

		public GameActionInfo(
			string name,
			ResourceRef parent,
			string tooltip,
			Drawable image,
			string[] verbs)
		{
			this.name = name;
			this.tooltip = tooltip;
			this.image = image;
			this.verbs = verbs;
			this.parent = parent;
		}

		public string Name
		{
			get { return name; }
		}

		public ResourceRef Parent
		{
			get { return parent; }
		}

		public string Tooltip
		{
			get { return tooltip; }
		}

		public Drawable Image
		{
			get { return image; }
		}

		public string[] Verbs
		{
			get { return verbs; }
		}
	}
}