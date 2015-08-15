using System.Drawing;
using OpenTK.Input;
using SharpHaven.Input;

namespace SharpHaven.UI.Widgets
{
	public class MapPlaceEvent
	{
		public MapPlaceEvent(MouseButtonEvent e, Point mapCoord)
			: this(e.Button, e.Modifiers, mapCoord)
		{
		}

		public MapPlaceEvent(MouseButton button, KeyModifiers mods, Point mapCoord)
		{
			MapCoord = mapCoord;
			Button = button;
			Modifiers = mods;
		}

		public Point MapCoord { get; }

		public MouseButton Button { get; }

		public KeyModifiers Modifiers { get; }
	}
}