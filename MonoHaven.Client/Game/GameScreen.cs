using System;
using System.Collections.Generic;
using System.Drawing;
using C5;
using MonoHaven.Login;
using MonoHaven.UI;
using MonoHaven.UI.Remote;
using MonoHaven.Utils;
using NLog;

namespace MonoHaven.Game
{
	public class GameScreen : BaseScreen
	{
		private static readonly NLog.Logger log = LogManager.GetCurrentClassLogger();

		private readonly GameSession session;
		private readonly TreeDictionary<ushort, Controller> controllers;
		private readonly WidgetAdapterRegistry adapterRegistry;

		public GameScreen(GameSession session)
		{
			this.session = session;
			this.controllers = new TreeDictionary<ushort, Controller>();
			this.controllers[0] = new Controller(0, session, RootWidget, new RootAdapter());
			this.adapterRegistry = new WidgetAdapterRegistry(session);
		}

		public EventHandler Closed;

		public void Close()
		{
			App.Instance.QueueOnMainThread(() => Host.SetScreen(new LoginScreen()));
		}

		public void CreateWidget(ushort id, string type, Point location, ushort parentId, object[] args)
		{
			var parent = FindController(parentId);
			if (parent == null)
				throw new Exception(
					string.Format("Non-existent parent widget {0} for {1}", parentId, id));
			var adapter = adapterRegistry.Get(type);
			var widget = adapter.Create(parent.Widget, args);
			widget.SetLocation(location);
			controllers[id] = new Controller(id, session, widget, adapter);
		}

		public void MessageWidget(ushort id, string message, object[] args)
		{
			var ctl = FindController(id);
			if (ctl == null)
			{
				log.Warn("UI message {1} to non-existent widget {0}", id, message);
				return;
			}
			ctl.HandleRemoteMessage(message, args);
		}

		public void DestroyWidget(ushort id)
		{
			Controller ctl;
			if (controllers.Remove(id, out ctl))
			{
				ctl.Widget.Remove();
				ctl.Widget.Dispose();
				return;
			}
			log.Warn("Try to remove non-existent widget {0}", id);
		}

		protected override void OnClose()
		{
			Closed.Raise(this, EventArgs.Empty);
		}

		private Controller FindController(ushort id)
		{
			Controller ctl;
			return controllers.Find(ref id, out ctl) ? ctl : null;
		}
	}
}
