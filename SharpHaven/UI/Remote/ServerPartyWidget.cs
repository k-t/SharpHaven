using System.Drawing;
using SharpHaven.UI.Widgets;

namespace SharpHaven.UI.Remote
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

		protected override void OnInit(Point position, object[] args)
		{
			widget = new PartyWidget(Parent.Widget, Session.Objects);
			widget.Move(position);
			widget.PlayerId = (int)args[0];
			widget.Party = Session.Party;
			widget.Leave += () => SendMessage("leave");
			widget.PartyMemberClick += OnPartyMemberClick;
		}

		private void OnPartyMemberClick(PartyMemberClickEvent e)
		{
			var button = ServerInput.ToServerButton(e.Button);
			SendMessage("click", e.MemberId, button);
		}
	}
}
