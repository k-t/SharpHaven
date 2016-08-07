using SharpHaven.Graphics;

namespace SharpHaven.Game.Messages
{
	public class WidgetCreate
	{
		public ushort Id { get; set; }

		public ushort ParentId { get; set; }

		public string Type { get; set; }

		public Coord2D Position { get; set; }

		public object[] Args { get; set; }
	}
}