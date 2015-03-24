using MonoHaven.UI.Widgets;

namespace MonoHaven.UI.Remote
{
	public class ServerPartyWidget : ServerWidget
	{
		public static ServerWidget Create(ushort id, ServerWidget parent, object[] args)
		{
			var widget = new PartyWidget(parent.Widget, parent.Session.State.Objects);
			widget.PlayerId = (int)args[0];
			return new ServerPartyWidget(id, parent, widget);
		}

		public ServerPartyWidget(ushort id, ServerWidget parent, PartyWidget widget)
			: base(id, parent, widget)
		{
			widget.Party = Session.State.Party;
			widget.Leave += () => SendMessage("leave");
			widget.PartyMemberClicked += OnPartyMemberClicked;
		}

		private void OnPartyMemberClicked(PartyMemberClickEventArgs e)
		{
			var button = ServerInput.ToServerButton(e.Button);
			SendMessage("click", e.MemberId, button);
		}
	}
}
