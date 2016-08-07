using SharpHaven.Graphics;

namespace SharpHaven.Game.Messages
{
	public class PartyUpdateMember
	{
		public int MemberId { get; set; }

		public Color Color { get; set; }

		public Coord2D? Location { get; set; }
	}
}