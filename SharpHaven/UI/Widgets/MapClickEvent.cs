using System;
using System.Drawing;
using OpenTK.Input;
using SharpHaven.Client;
using SharpHaven.Input;

namespace SharpHaven.UI.Widgets
{
	public class MapClickEvent : EventArgs
	{
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
			Button = button;
			MapCoord = mapCoord;
			ScreenCoord = screenCoord;
			Gob = gob;
			Modifiers = mods;
		}

		public MouseButton Button { get; }

		public Gob Gob { get; }

		public Point MapCoord { get; }

		public Point ScreenCoord { get; }

		public KeyModifiers Modifiers { get; }
	}
}
