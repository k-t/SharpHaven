using SharpHaven.Graphics;

namespace SharpHaven.Game.Events
{
	public class MapUpdateEvent
	{
		public Coord2D Grid
		{
			get;
			set;
		}

		public string MinimapName
		{
			get;
			set;
		}

		public byte[] Tiles
		{
			get;
			set;
		}

		public int[] Overlays
		{
			get;
			set;
		}
	}
}
