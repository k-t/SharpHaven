using MonoHaven.UI.Widgets;

namespace MonoHaven.UI.Remote
{
	public class ServerPartyWidget : ServerWidget
	{
		private PartyWidget widget;

		public ServerPartyWidget(ushort id, ServerWidget parent) : base(id, parent)
		{
		}

		public override Widget Widget
		{
			get { return widget; }
		}

		public static ServerWidget Create(ushort id, ServerWidget parent)
		{
			return new ServerPartyWidget(id, parent);
		}

		protected override void OnInit(object[] args)
		{
			widget = new PartyWidget(Parent.Widget, Session.State.Objects);
			widget.PlayerId = (int)args[0];
			widget.Party = Session.State.Party;
			widget.Leave += () => SendMessage("leave");
			widget.PartyMemberClick += OnPartyMemberClick;
		}

		private void OnPartyMemberClick(PartyMemberClickEventArgs e)
		{
			var button = ServerInput.ToServerButton(e.Button);
			SendMessage("click", e.MemberId, button);
		}
	}
}
