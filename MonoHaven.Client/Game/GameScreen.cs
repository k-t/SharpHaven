using System;
using System.Collections.Generic;
using System.Drawing;
using MonoHaven.Login;
using MonoHaven.UI;
using MonoHaven.UI.Remote;
using NLog;

namespace MonoHaven.Game
{
	public class GameScreen : BaseScreen
	{
		private static readonly Logger log = LogManager.GetCurrentClassLogger();

		private readonly Dictionary<int, Controller> controllers;
		private readonly WidgetAdapterRegistry adapterRegistry;

		public GameScreen(WidgetAdapterRegistry adapterRegistry)
		{
			this.controllers = new Dictionary<int, Controller>();
			this.controllers[0] = new Controller(0, RootWidget, new RootAdapter());
			this.adapterRegistry = adapterRegistry;
		}

		public void Close()
		{
			App.Instance.QueueOnMainThread(() => Host.SetScreen(new LoginScreen()));
		}

		public void CreateWidget(int id, string type, Point location, int parentId, object[] args)
		{
			var parent = FindController(parentId);
			if (parent == null)
				throw new Exception(
					string.Format("Non-existent parent widget {0} for {1}", parentId, id));
			var adapter = adapterRegistry.Get(type);
			var widget = adapter.Create(parent.Widget, args);
			widget.SetLocation(location);
			controllers[id] = new Controller(id, widget, adapter);
		}

		public void MessageWidget(int id, string message, object[] args)
		{
			var ctl = FindController(id);
			if (ctl == null)
			{
				log.Warn("UI message {1} to non-existent widget {0}", id, message);
				return;
			}
			ctl.HandleRemoteMessage(message, args);
		}

		public void DestroyWidget(int id)
		{
			if (controllers.Remove(id))
			{
				return;
			}
			log.Warn("Try to remove non-existent widget {0}", id);
		}

		private Controller FindController(int id)
		{
			Controller ctl;
			return controllers.TryGetValue(id, out ctl) ? ctl : null;
		}
	}
}
