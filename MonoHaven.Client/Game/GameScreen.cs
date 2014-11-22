#region License
// Copyright (c) 2014 Marat Vildanov <marat.vildanov@gmail.com>
// Distributed under the MIT License.
// (See accompanying LICENSE file or copy at http://opensource.org/licenses/MIT)
#endregion

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

		private Container charlistContainer;
		private MapView mapView;
		private Calendar calendar;

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

		protected override void OnUpdate(int dt)
		{
			session.State.Objects.Tick(dt);
		}

		protected override void OnResize(int newWidth, int newHeight)
		{
			if (mapView != null)
				mapView.SetSize(newWidth, newHeight);

			if (charlistContainer != null)
				charlistContainer.SetLocation(
					(newWidth - charlistContainer.Width) / 2,
					(newHeight - charlistContainer.Height) / 2);

			if (calendar != null)
				calendar.SetLocation((Window.Width - calendar.Width) / 2, calendar.Y);
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
			HandleCreatedWidget(swidget.Widget);
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
					HandleDestroyedWidget(widget.Widget);
				}
				HandleDestroyedWidget(widget.Widget);
				return;
			}
			log.Warn("Try to remove non-existent widget {0}", message.Id);
		}

		private void OnSessionFinished()
		{
			App.Instance.QueueOnMainThread(() => Exited.Raise());
		}

		private ServerWidget GetWidget(ushort id)
		{
			ServerWidget widget;
			return serverWidgets.Find(ref id, out widget) ? widget : null;
		}

		private void HandleCreatedWidget(Widget widget)
		{
			if (widget is MapView)
			{
				mapView = (MapView)widget;
				mapView.SetSize(Window.Width, Window.Height);
			}
			if (widget is Container)
			{
				charlistContainer = (Container)widget;
				charlistContainer.SetLocation(
					(Window.Width - charlistContainer.Width) / 2,
					(Window.Height - charlistContainer.Height) / 2);
			}
			if (widget is Calendar)
			{
				calendar = (Calendar)widget;
				calendar.SetLocation((Window.Width - calendar.Width) / 2, calendar.Y);
			}
		}

		private void HandleDestroyedWidget(Widget widget)
		{
			if (widget == mapView) mapView = null;
			if (widget == charlistContainer) charlistContainer = null;
			if (widget == calendar) calendar = null;
		}
	}
}
