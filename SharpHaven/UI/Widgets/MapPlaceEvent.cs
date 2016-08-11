using Haven;
using OpenTK.Input;
using SharpHaven.Input;

namespace SharpHaven.UI.Widgets
{
	public class MapPlaceEvent
	{
		public MapPlaceEvent(MouseButtonEvent e, Point2D mapCoord)
			: this(e.Button, e.Modifiers, mapCoord)
		{
		}

		public MapPlaceEvent(MouseButton button, KeyModifiers mods, Point2D mapCoord)
		{
			MapCoord = mapCoord;
			Button = button;
			Modifiers = mods;
		}

		public Point2D MapCoord { get; }

		public MouseButton Button { get; }

		public KeyModifiers Modifiers { get; }
	}
}