namespace MonoHaven.UI.Remote
{
	public class ServerEquipory : ServerWidget
	{
		public static ServerWidget Create(ushort id, ServerWidget parent, object[] args)
		{
			var widget = new Window(parent.Widget, "Equipment");
			return new ServerEquipory(id, parent, widget);
		}

		public ServerEquipory(ushort id, ServerWidget parent, Widget widget)
			: base(id, parent, widget)
		{
		}
	}
}
