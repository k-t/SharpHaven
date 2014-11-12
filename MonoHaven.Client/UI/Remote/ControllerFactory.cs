using System;
using System.Collections.Generic;
using MonoHaven.Game;

namespace MonoHaven.UI.Remote
{
	public class ControllerFactory
	{
		public delegate Controller CreateDelegate(int id, Controller parent, object[] args);

		private readonly GameSession session;
		private readonly Dictionary<string, CreateDelegate> delegates;

		public ControllerFactory(GameSession session)
		{
			this.session = session;
			delegates = new Dictionary<string, CreateDelegate>();
			delegates["cnt"] = ContainerController.Create;
			delegates["img"] = ImageController.Create;
			delegates["charlist"] = CharlistController.Create;
			delegates["ibtn"] = ImageButtonController.Create;
		}

		public Controller Create(int id, string type, Controller parent, object[] args)
		{
			CreateDelegate create;

			if (!delegates.TryGetValue(type, out create))
				throw new Exception("Unknown widget type " + type);

			var ctl = create(id, parent, args);
			ctl.Session = session;
			return ctl;
		}
	}
}
