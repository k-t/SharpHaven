using System;
using System.Collections.Generic;
using MonoHaven.Game;

namespace MonoHaven.UI.Remote
{
	public class Controller : IDisposable
	{
		private readonly ushort id;
		private readonly List<Controller> children;
		private readonly GameSession session;
		private Widget widget;
		private WidgetAdapter adapter;

		public Controller(ushort id, GameSession session)
		{
			this.id = id;
			this.session = session;
			this.children = new List<Controller>();
		}

		public Controller(ushort id, Controller parent)
			: this(id, parent.session)
		{
			parent.AddChild(this);
		}

		public ushort Id
		{
			get { return id; }
		}

		public IEnumerable<Controller> Children
		{
			get { return children; }
		}

		public Widget Widget
		{
			get { return widget; }
		}

		public void Bind(Widget widget, WidgetAdapter adapter)
		{
			this.widget = widget;
			this.adapter = adapter;
			adapter.SetEventHandler(widget, HandleWidgetMessage);
		}

		public void HandleRemoteMessage(string message, object[] args)
		{
			adapter.HandleMessage(widget, message, args);
		}

		private void HandleWidgetMessage(string message, object[] args)
		{
			session.SendWidgetMessage(id, message, args);
		}

		private void AddChild(Controller child)
		{
			children.Add(child);
		}

		public void Dispose()
		{
			widget.Remove();
			widget.Dispose();
		}
	}
}
