using System.Drawing;

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

		public Point? Location
		{
			get;
			set;
		}
	}
}
