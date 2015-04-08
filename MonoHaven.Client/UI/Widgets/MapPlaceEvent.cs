using System.Drawing;
using MonoHaven.Input;
using OpenTK.Input;

namespace MonoHaven.UI.Widgets
{
	public class MapPlaceEvent
	{
		private readonly Point mapCoord;
		private readonly MouseButton button;
		private readonly KeyModifiers mods;

		public MapPlaceEvent(MouseButtonEvent e, Point mapCoord)
			: this(e.Button, e.Modifiers, mapCoord)
		{
		}

		public MapPlaceEvent(MouseButton button, KeyModifiers mods, Point mapCoord)
		{
			this.mapCoord = mapCoord;
			this.button = button;
			this.mods = mods;
		}

		public Point MapCoord
		{
			get { return mapCoord; }
		}

		public MouseButton Button
		{
			get { return button; }
		}

		public KeyModifiers Modifiers
		{
			get { return mods; }
		}
	}
}