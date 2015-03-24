using System.Drawing;
using OpenTK.Input;

namespace MonoHaven.UI.Widgets
{
	public class MapPlaceEventArgs
	{
		private readonly Point point;
		private readonly MouseButton button;
		private readonly Input.KeyModifiers mods;

		public MapPlaceEventArgs(Point point, MouseButton button, Input.KeyModifiers mods)
		{
			this.point = point;
			this.button = button;
			this.mods = mods;
		}

		public Point Point
		{
			get { return point; }
		}

		public MouseButton Button
		{
			get { return button; }
		}

		public Input.KeyModifiers Modifiers
		{
			get { return mods; }
		}
	}
}