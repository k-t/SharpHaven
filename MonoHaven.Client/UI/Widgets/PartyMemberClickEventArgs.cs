using OpenTK.Input;

namespace MonoHaven.UI.Widgets
{
	public class PartyMemberClickEventArgs
	{
		private readonly int memberId;
		private readonly MouseButton button;

		public PartyMemberClickEventArgs(int memberId, MouseButton button)
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