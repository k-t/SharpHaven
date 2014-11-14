﻿using System;
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

		private readonly ServerWidgetFactory factory;
		private readonly IDictionary<ushort, TreeNode<ServerWidget>> serverWidgets;

		public GameScreen(GameSession session)
		{
			factory = new ServerWidgetFactory();
			serverWidgets = new TreeDictionary<ushort, TreeNode<ServerWidget>>();
			serverWidgets[0] = new TreeNode<ServerWidget>(new ServerRootWidget(0, session, RootWidget));
		}

		public EventHandler Closed;

		public void Close()
		{
			App.Instance.QueueOnMainThread(() => Host.SetScreen(new LoginScreen()));
		}

		public void CreateWidget(ushort id, string type, Point location, ushort parentId, object[] args)
		{
			var parent = GetWidget(parentId);
			if (parent == null)
				throw new Exception(
					string.Format("Non-existent parent widget {0} for {1}", parentId, id));

			var swidget = factory.Create(type, id, parent.Value, args);
			swidget.Widget.SetLocation(location);

			var node = new TreeNode<ServerWidget>(swidget);
			parent.AddChild(node);
			serverWidgets[id] = node;
		}

		public void MessageWidget(ushort id, string message, object[] args)
		{
			var widget = GetWidget(id);
			if (widget != null)
				widget.Value.ReceiveMessage(message, args);
			else
				log.Warn("UI message {1} to non-existent widget {0}", id, message);
		}

		public void DestroyWidget(ushort id)
		{
			TreeNode<ServerWidget> widget;
			if (serverWidgets.Remove(id, out widget))
			{
				widget.Remove();
				widget.Value.Dispose();
				foreach (var child in widget.Descendants)
				{
					serverWidgets.Remove(child.Value.Id);
					child.Value.Dispose();
				}
				return;
			}
			log.Warn("Try to remove non-existent widget {0}", id);
		}

		protected override void OnClose()
		{
			Closed.Raise(this, EventArgs.Empty);
		}

		private TreeNode<ServerWidget> GetWidget(ushort id)
		{
			TreeNode<ServerWidget> widget;
			return serverWidgets.Find(ref id, out widget) ? widget : null;
		}
	}
}
