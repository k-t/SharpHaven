using SharpHaven.Graphics;

namespace SharpHaven.Game.Messages
{
	public class MapRequestGrid
	{
		public MapRequestGrid()
		{
		}

		public MapRequestGrid(Coord2D coord)
		{
			Coord = coord;
		}

		public Coord2D Coord { get; set; }
	}
}
