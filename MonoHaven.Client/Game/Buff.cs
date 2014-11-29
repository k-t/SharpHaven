﻿using MonoHaven.Graphics;

namespace MonoHaven.Game
{
	public class Buff
	{
		public int Id { get; set; }
		public Delayed<Drawable> Image { get; set; }
		public string Tooltip { get; set; }
		public int AMeter { get; set; }
		public int NMeter { get; set; }
		public int CMeter { get; set; }
		public int CTicks { get; set; }
		public long Time { get; set; }
		public bool Major { get; set; }
	}
}