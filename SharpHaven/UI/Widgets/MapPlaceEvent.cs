﻿using OpenTK.Input;
using SharpHaven.Graphics;
using SharpHaven.Input;

namespace SharpHaven.UI.Widgets
{
	public class MapPlaceEvent
	{
		public MapPlaceEvent(MouseButtonEvent e, Coord2D mapCoord)
			: this(e.Button, e.Modifiers, mapCoord)
		{
		}

		public MapPlaceEvent(MouseButton button, KeyModifiers mods, Coord2D mapCoord)
		{
			MapCoord = mapCoord;
			Button = button;
			Modifiers = mods;
		}

		public Coord2D MapCoord { get; }

		public MouseButton Button { get; }

		public KeyModifiers Modifiers { get; }
	}
}