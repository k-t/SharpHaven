using System;
using C5;
using MonoHaven.Game.Messages;
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
		private readonly ServerWidgetFactory factory;
		private readonly IDictionary<ushort, ServerWidget> serverWidgets;

		public GameScreen(GameSession session)
		{
			this.session = session;
			this.session.WidgetCreated += OnWidgetCreated;
			this.session.WidgetDestroyed += OnWidgetDestroyed;
			this.session.WidgetMessage += OnWidgetMessage;
			this.session.Finished += OnSessionFinished;

			factory = new ServerWidgetFactory();
			serverWidgets = new TreeDictionary<ushort, ServerWidget>();
			serverWidgets[0] = new ServerRootWidget(0, session, RootWidget);
		}

		public Action Exited;

		protected override void OnClose()
		{
			session.Finish();
		}

		private ServerWidget GetWidget(ushort id)
		{
			ServerWidget widget;
			return serverWidgets.Find(ref id, out widget) ? widget : null;
		}

		private void OnWidgetCreated(WidgetCreateMessage message)
		{
			var parent = GetWidget(message.ParentId);
			if (parent == null)
				throw new Exception(string.Format(
					"Non-existent parent widget {0} for {1}",
					message.ParentId,
					message.Id));

			var swidget = factory.Create(message.Type, message.Id, parent, message.Args);
			swidget.Widget.SetLocation(message.Location);
			serverWidgets[message.Id] = swidget;
		}

		private void OnWidgetMessage(WidgetMessage message)
		{
			var widget = GetWidget(message.Id);
			if (widget != null)
				widget.ReceiveMessage(message.Name, message.Args);
			else
				log.Warn("UI message {1} to non-existent widget {0}",
					message.Id, message.Name);
		}

		private void OnWidgetDestroyed(WidgetDestroyMessage message)
		{
			ServerWidget widget;
			if (serverWidgets.Remove(message.Id, out widget))
			{
				widget.Remove();
				widget.Dispose();
				foreach (var child in widget.Descendants)
				{
					serverWidgets.Remove(child.Id);
					child.Dispose();
				}
				return;
			}
			log.Warn("Try to remove non-existent widget {0}", message.Id);
		}

		private void OnSessionFinished()
		{
			App.Instance.QueueOnMainThread(() => Exited.Raise());
		}
	}
}
