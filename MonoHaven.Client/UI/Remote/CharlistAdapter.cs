using System.Collections.Generic;
using MonoHaven.Game;
using MonoHaven.Resources;

namespace MonoHaven.UI.Remote
{
	public class CharlistAdapter : WidgetAdapter
	{
		private readonly GameSession session;

		public CharlistAdapter(GameSession session)
		{
			this.session = session;
		}

		public override Widget Create(Widget parent, object[] args)
		{
			int height = args.Length > 0 ? (int)args[0] : 0;
			return new Charlist(parent, height);
		}

		public override void HandleMessage(Widget widget, string message, object[] args)
		{
			var charlist = (Charlist)widget;
			if (message == "add")
			{
				var name = (string)args[0];
				var layers = new List<Resource>();
				for (int i = 1; i < args.Length; i++)
					layers.Add(session.GetResource((int)args[i]));
				charlist.AddChar(name, layers);
			}
			else
				base.HandleMessage(widget, message, args);
		}
	}
}
