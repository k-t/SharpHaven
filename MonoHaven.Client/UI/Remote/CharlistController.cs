using System.Collections.Generic;
using MonoHaven.Game;
using MonoHaven.Resources;

namespace MonoHaven.UI.Remote
{
	public class CharlistController : Controller
	{
		private readonly Charlist widget;

		private CharlistController(int id, GameSession session, Charlist widget)
			: base(id, session)
		{
			this.widget = widget;
		}

		public override Widget Widget
		{
			get { return widget; }
		}

		public override void HandleMessage(string message, object[] args)
		{
			if (message == "add")
			{
				var name = (string)args[0];
				var layers = new List<Resource>();
				for(int i = 1; i < args.Length; i++)
					layers.Add(Session.GetResource((int)args[i]));
				widget.AddChar(name, layers);
				return;
			}
			base.HandleMessage(message, args);
		}

		public static Controller Create(int id, GameSession session, Controller parent, object[] args)
		{
			int height = args.Length > 0 ? (int)args[0] : 0;
			return new CharlistController(id, session, new Charlist(parent.Widget, height));
		}
	}
}
