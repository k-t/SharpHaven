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

		private readonly GameSession session;
		private readonly Dictionary<int, Controller> controllers;
		private readonly ControllerFactory controllerFactory;

		public GameScreen(GameSession session)
		{
			this.session = session;
			this.controllers = new Dictionary<int, Controller>();
			this.controllers[0] = new RootController(0, session, RootWidget);
			this.controllerFactory = new ControllerFactory();
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

			var ctl = controllerFactory.Create(id, session, type, parent, args);
			ctl.Widget.SetLocation(location);
			controllers[id] = ctl;
		}

		public void MessageWidget(int id, string message, object[] args)
		{
			var ctl = FindController(id);
			if (ctl == null)
			{
				log.Warn("UI message {1} to non-existent widget {0}", id, message);
				return;
			}
			ctl.HandleMessage(message, args);
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
