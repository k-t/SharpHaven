using System;
using MonoHaven.Game;
using MonoHaven.Utils;
using NLog;

namespace MonoHaven.UI.Remote
{
	public abstract class ServerWidget : TreeNode<ServerWidget>, IDisposable
	{
		private readonly static Logger log = LogManager.GetCurrentClassLogger();

		private readonly ushort id;
		private readonly GameSession session;
		private readonly Widget widget;

		protected ServerWidget(ushort id, GameSession session, Widget widget)
		{
			this.id = id;
			this.widget = widget;
			this.session = session;
		}

		protected ServerWidget(ushort id, ServerWidget parent, Widget widget)
			: this(id, parent.Session, widget)
		{
			parent.AddChild(this);
		}

		public ushort Id
		{
			get { return id; }
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
				message, string.Join(",", args), GetType());
		}

		protected void SendMessage(string message)
		{
			SendMessage(message, new object[0]);
		}

		protected void SendMessage(string message, params object[] args)
		{
			session.SendMessage(id, message, args);
		}

		public void Dispose()
		{
			widget.Remove();
			widget.Dispose();
		}
	}
}
