using System.Drawing;
using MonoHaven.Input;
using OpenTK.Input;

namespace MonoHaven.UI.Widgets
{
	public class MapPlaceEventArgs
	{
		private readonly Point mapCoord;
		private readonly MouseButton button;
		private readonly KeyModifiers mods;

		public MapPlaceEventArgs(MouseButtonEvent e, Point mapCoord)
			: this(e.Button, e.Modifiers, mapCoord)
		{
		}

		public MapPlaceEventArgs(MouseButton button, KeyModifiers mods, Point mapCoord)
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