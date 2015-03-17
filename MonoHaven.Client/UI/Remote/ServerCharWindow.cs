using MonoHaven.UI.Widgets;

namespace MonoHaven.UI.Remote
{
	public class ServerCharWindow : ServerWindow
	{
		public static ServerWidget Create(ushort id, ServerWidget parent, object[] args)
		{
			var studyId = args.Length > 0 ? (int)args[0] : -1;
			var widget = new CharWindow(parent.Widget, parent.Session.State);
			var serverWidget = new ServerCharWindow(id, parent, widget);

			// HACK: study widget is not created through a server message
			//       but passed in the arguments to the char window
			if (studyId != -1)
			{
				var study = new ServerContainer((ushort)studyId, serverWidget, widget.Study);
				parent.Session.Screen.Bind(study.Id, study);
			}
				
			return serverWidget;
		}

		private readonly CharWindow widget;

		public ServerCharWindow(ushort id, ServerWidget parent, CharWindow widget)
			: base(id, parent, widget)
		{
			this.widget = widget;
			this.widget.AttributesBought += args => SendMessage("sattr", args);
		}

		public override void ReceiveMessage(string message, object[] args)
		{
			if (message == "exp")
				widget.SetExp((int)args[0]);
			else if (message == "food")
				widget.FoodMeter.Update(args);
		}
	}
}
