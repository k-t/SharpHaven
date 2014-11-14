using System;
using System.Collections.Generic;
using MonoHaven.Game;
using NLog;

namespace MonoHaven.UI.Remote
{
	public abstract class ServerWidget : IDisposable
	{
		private readonly static Logger log = LogManager.GetCurrentClassLogger();

		private readonly ushort id;
		private readonly ServerWidget parent;
		private readonly List<ServerWidget> children;
		private readonly GameSession session;
		private readonly Widget widget;

		public ServerWidget(ushort id, GameSession session, Widget widget)
		{
			this.id = id;
			this.widget = widget;
			this.session = session;
			this.children = new List<ServerWidget>();
		}

		public ServerWidget(ushort id, ServerWidget parent, Widget widget)
			: this(id, parent.Session, widget)
		{
			this.parent = parent;
			this.parent.AddChild(this);
		}

		public ushort Id
		{
			get { return id; }
		}

		public IEnumerable<ServerWidget> Children
		{
			get { return children; }
		}

		public GameSession Session
		{
			get { return session; }
		}

		public Widget Widget
		{
			get { return widget; }
		}

		public virtual void ReceiveMessage(string message, object[] args)
		{
			// TODO: handle common widget commands (focus, tab, etc).

			log.Warn("Unhandled message {0}({1}) for {2}",
				message, string.Join(",", args), widget.GetType());
		}

		protected void SendMessage(string message)
		{
			SendMessage(message, new object[0]);
		}

		protected void SendMessage(string message, params object[] args)
		{
			session.SendWidgetMessage(id, message, args);
		}

		private void AddChild(ServerWidget child)
		{
			children.Add(child);
		}

		public void Dispose()
		{
			widget.Remove();
			widget.Dispose();
			parent.children.Remove(this);
		}
	}
}
