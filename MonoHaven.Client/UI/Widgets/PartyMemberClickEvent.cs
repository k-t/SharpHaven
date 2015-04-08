using OpenTK.Input;

namespace MonoHaven.UI.Widgets
{
	public class PartyMemberClickEvent
	{
		private readonly int memberId;
		private readonly MouseButton button;

		public PartyMemberClickEvent(int memberId, MouseButton button)
		{
			this.memberId = memberId;
			this.button = button;
		}

		public int MemberId
		{
			get { return memberId; }
		}

		public MouseButton Button
		{
			get { return button; }
		}
	}
}