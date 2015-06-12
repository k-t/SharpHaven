using System.Drawing;

namespace SharpHaven.Game.Events
{
	public class MapInvalidateRegionEvent
	{
		public Rectangle Region { get; set; }
	}
}
