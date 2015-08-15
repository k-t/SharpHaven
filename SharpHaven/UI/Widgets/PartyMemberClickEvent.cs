using OpenTK.Input;

namespace SharpHaven.UI.Widgets
{
	public class PartyMemberClickEvent
	{
		public PartyMemberClickEvent(int memberId, MouseButton button)
		{
			MemberId = memberId;
			Button = button;
		}

		public int MemberId { get; }

		public MouseButton Button { get; }
	}
}