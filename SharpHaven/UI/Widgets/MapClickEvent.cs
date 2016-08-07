using System;
using OpenTK.Input;
using SharpHaven.Client;
using SharpHaven.Graphics;
using SharpHaven.Input;

namespace SharpHaven.UI.Widgets
{
	public class MapClickEvent : EventArgs
	{
		public MapClickEvent(MouseButtonEvent e, Coord2D mapCoord, Coord2D screenCoord, Gob gob)
			: this(e.Button, e.Modifiers, mapCoord, screenCoord, gob)
		{
		}

		public MapClickEvent(
			MouseButton button,
			KeyModifiers mods,
			Coord2D mapCoord,
			Coord2D screenCoord,
			Gob gob)
		{
			Button = button;
			MapCoord = mapCoord;
			ScreenCoord = screenCoord;
			Gob = gob;
			Modifiers = mods;
		}

		public MouseButton Button { get; }

		public Gob Gob { get; }

		public Coord2D MapCoord { get; }

		public Coord2D ScreenCoord { get; }

		public KeyModifiers Modifiers { get; }
	}
}
