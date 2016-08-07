using System;
using OpenTK.Input;
using SharpHaven.Client;
using SharpHaven.Graphics;
using SharpHaven.Input;

namespace SharpHaven.UI.Widgets
{
	public class MapClickEvent : EventArgs
	{
		public MapClickEvent(MouseButtonEvent e, Coord2d mapCoord, Coord2d screenCoord, Gob gob)
			: this(e.Button, e.Modifiers, mapCoord, screenCoord, gob)
		{
		}

		public MapClickEvent(
			MouseButton button,
			KeyModifiers mods,
			Coord2d mapCoord,
			Coord2d screenCoord,
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

		public Coord2d MapCoord { get; }

		public Coord2d ScreenCoord { get; }

		public KeyModifiers Modifiers { get; }
	}
}
