namespace MonoHaven.UI.Remote
{
	public class ServerEquipory : ServerWindow
	{
		public static ServerWidget Create(ushort id, ServerWidget parent, object[] args)
		{
			var widget = new Equipory(parent.Widget, parent.Session.State.Objects);
			return new ServerEquipory(id, parent, widget);
		}

		private readonly Equipory widget;

		public ServerEquipory(ushort id, ServerWidget parent, Equipory widget)
			: base(id, parent, widget)
		{
			this.widget = widget;
		}

		public override void ReceiveMessage(string message, object[] args)
		{
			if (message == "ava")
				widget.SetGob((int)args[0]);
			else if (message == "set")
			{
				int i = 0;
				int j = 0;
				while (j < args.Length)
				{
					var resId = (int)args[j++];
					if (resId >= 0)
					{
						var q = (int)args[j++];
						var tooltip = (j < args.Length && args[j] is string)
							? (string)args[j++]
							: null;
						widget.SetItem(i, Session.GetImage(resId), tooltip);
					}
					else
						widget.SetItem(i, null, null);
					i++;
				}
			}
			else if (message == "setres")
			{
				var i = (int)args[0];
				var resId = (int)args[1];
				var q = (int)args[2];
				// TODO: tooltip must be preserved
				widget.SetItem(i, Session.GetImage(resId), null);
			}
			else
				base.ReceiveMessage(message, args);
		}
	}
}
