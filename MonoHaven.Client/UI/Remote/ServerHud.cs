namespace MonoHaven.UI.Remote
{
	public class ServerHud : ServerWidget
	{
		public static ServerWidget Create(ushort id, ServerWidget parent, object[] args)
		{
			var widget = new Hud(parent.Widget);
			return new ServerHud(id, parent, widget);
		}

		public ServerHud(ushort id, ServerWidget parent, Hud widget)
			: base(id, parent, widget)
		{
		}
	}
}
