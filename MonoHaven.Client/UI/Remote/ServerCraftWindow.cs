using MonoHaven.UI.Widgets;

namespace MonoHaven.UI.Remote
{
	public class ServerCraftWindow : ServerWindow
	{
		public static new ServerWidget Create(ushort id, ServerWidget parent, object[] args)
		{
			var recipeName = (string)args[0];

			var widget = new CraftWindow(parent.Widget);
			widget.RecipeName = recipeName;
			return new ServerCraftWindow(id, parent, widget);
		}

		private readonly CraftWindow widget;

		public ServerCraftWindow(ushort id, ServerWidget parent, CraftWindow widget)
			: base(id, parent, widget)
		{
			this.widget = widget;
			this.widget.Craft += () => SendMessage("make", 0);
			this.widget.CraftAll += () => SendMessage("make", 1);
		}

		public override void ReceiveMessage(string message, object[] args)
		{
			if (message == "pop")
			{
				widget.Clear();
				int i;
				for (i = 0; (int)args[i] >= 0; i += 2)
				{
					var image = Session.GetImage((int)args[i]);
					var count = (int)args[i + 1];
					widget.AddInput(image, count);
				}
				for (i++; (i < args.Length) && ((int)args[i] >= 0); i += 2)
				{
					var image = Session.GetImage((int)args[i]);
					var count = (int)args[i + 1];
					widget.AddOutput(image, count);
				}
			}
			else
				base.ReceiveMessage(message, args);
		}
	}
}
