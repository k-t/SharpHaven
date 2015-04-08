using System;
using System.Drawing;
using MonoHaven.Game;
using MonoHaven.Input;
using OpenTK.Input;

namespace MonoHaven.UI.Widgets
{
	public class MapClickEvent : EventArgs
	{
		private readonly MouseButton button;
		private readonly KeyModifiers mods;
		private readonly Point mapCoord;
		private readonly Point screenCoord;
		private readonly Gob gob;

		public MapClickEvent(MouseButtonEvent e, Point mapCoord, Point screenCoord, Gob gob)
			: this(e.Button, e.Modifiers, mapCoord, screenCoord, gob)
		{
		}

		public MapClickEvent(
			MouseButton button,
			KeyModifiers mods,
			Point mapCoord,
			Point screenCoord,
			Gob gob)
		{
			this.button = button;
			this.mapCoord = mapCoord;
			this.screenCoord = screenCoord;
			this.gob = gob;
			this.mods = mods;
		}

		public MouseButton Button
		{
			get { return button; }
		}

		public Gob Gob
		{
			get { return gob; }
		}

		public Point MapCoord
		{
			get { return mapCoord; }
		}

		public Point ScreenCoord
		{
			get { return screenCoord; }
		}

		public KeyModifiers Modifiers
		{
			get { return mods; }
		}
	}
}
