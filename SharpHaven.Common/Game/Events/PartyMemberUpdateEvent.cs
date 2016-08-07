using SharpHaven.Graphics;

namespace SharpHaven.Game.Events
{
	public class PartyMemberUpdateEvent
	{
		public int MemberId
		{
			get;
			set;
		}

		public Color Color
		{
			get;
			set;
		}

		public Coord2D? Location
		{
			get;
			set;
		}
	}
}
