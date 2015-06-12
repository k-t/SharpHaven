using System.Drawing;

namespace SharpHaven.Client
{
	public class MapOverlay
	{
		private readonly int index;

		public MapOverlay(int index)
		{
			this.index = index;
		}

		public int Index
		{
			get { return index; }
		}

		public Rectangle Bounds
		{
			get;
			set;
		}
	}
}
