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
		
		private readonly GameState gstate;
		private readonly Dictionary<int, RemoteWidget> rwidgets;
		private readonly RemoteWidgetFactory rwidgetFactory;

		public GameScreen(GameState gstate)
		{
			this.gstate = gstate;
			this.rwidgets = new Dictionary<int, RemoteWidget>();
			this.rwidgets[0] = new RemoteRoot(0, RootWidget);
			this.rwidgetFactory = new RemoteWidgetFactory();
		}

		public void Close()
		{
			App.Instance.QueueOnMainThread(() => Host.SetScreen(new LoginScreen()));
		}

		public void CreateWidget(int id, string type, Point location, int parentId, object[] args)
		{
			var parent = FindWidget(parentId);
			if (parent == null)
				throw new Exception(
					string.Format("Non-existent parent widget {0} for {1}", parentId, id));

			var rwidget = rwidgetFactory.Create(id, type, parent, args);
			rwidget.Widget.SetLocation(location);
			rwidgets[id] = rwidget;
		}

		public void MessageWidget(int id, string message, object[] args)
		{
			var rwidget = FindWidget(id);
			if (rwidget == null)
			{
				log.Warn("UI message {1} to non-existent widget {0}", id, message);
				return;
			}
			rwidget.HandleMessage(message, args);
		}

		public void DestroyWidget(int id)
		{
			if (rwidgets.Remove(id))
			{
				return;
			}
			log.Warn("Try to remove non-existent widget {0}", id);
		}

		private RemoteWidget FindWidget(int id)
		{
			RemoteWidget widget;
			return rwidgets.TryGetValue(id, out widget) ? widget : null;
		}
	}
}
