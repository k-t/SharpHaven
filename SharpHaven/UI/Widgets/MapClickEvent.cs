using System;
using Haven;
using OpenTK.Input;
using SharpHaven.Client;
using SharpHaven.Input;

namespace SharpHaven.UI.Widgets
{
	public class MapClickEvent : EventArgs
	{
		public MapClickEvent(MouseButtonEvent e, Point2D mapCoord, Point2D screenCoord, Gob gob)
			: this(e.Button, e.Modifiers, mapCoord, screenCoord, gob)
		{
		}

		public MapClickEvent(
			MouseButton button,
			KeyModifiers mods,
			Point2D mapCoord,
			Point2D screenCoord,
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

		public Point2D MapCoord { get; }

		public Point2D ScreenCoord { get; }

		public KeyModifiers Modifiers { get; }
	}
}
