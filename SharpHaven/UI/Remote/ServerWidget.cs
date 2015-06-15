using System;
using System.Collections.Generic;
using System.Drawing;
using NLog;
using OpenTK;
using SharpHaven.Client;
using SharpHaven.UI.Widgets;
using SharpHaven.Utils;

namespace SharpHaven.UI.Remote
{
	public abstract class ServerWidget : TreeNode<ServerWidget>, IDisposable
	{
		private readonly static Logger Log = LogManager.GetCurrentClassLogger();

		private readonly ushort id;
		private readonly ClientSession session;
		private readonly Dictionary<string, Action<object[]>> handlers;

		protected ServerWidget(ushort id, ClientSession session)
		{
			this.id = id;
			this.session = session;
			this.handlers = new Dictionary<string, Action<object[]>>();

			// TODO: handle common widget commands (focus, tab, etc).
			SetHandler("curs", SetCursor);
		}

		protected ServerWidget(ushort id, ServerWidget parent)
			: this(id, parent.Session)
		{
			parent.AddChild(this);
		}

		public ushort Id
		{
			get { return id; }
		}

		public ClientSession Session
		{
			get { return session; }
		}

		public abstract Widget Widget
		{
			get;
		}

		public void Init(Point position, object[] args)
		{
			OnInit(position, args);
		}

		public void Dispose()
		{
			OnDestroy();

			if (Widget != null)
			{
				Widget.Remove();
				Widget.Dispose();
			}
		}

		public void HandleMessage(string message, object[] args)
		{
			Action<object[]> handler;
			if (handlers.TryGetValue(message, out handler))
				handler(args);
			else
				Log.Warn("Unhandled message {0}({1}) for {2}",
					message, string.Join(",", args), GetType());
		}

		protected virtual void OnInit(Point position, object[] args)
		{
		}

		protected virtual void OnDestroy()
		{
		}

		protected void SetHandler(string messageName, Action<object[]> handler)
		{
			handlers[messageName] = handler;
		}

		protected void SendMessage(string message, params object[] args)
		{
			session.MessageWidget(id, message, args ?? new object[0]);
		}

		private void SetCursor(object[] args)
		{
			if (Widget == null)
				return;

			Widget.Cursor = args.Length > 0
				? App.Resources.Get<MouseCursor>((string)args[0])
				: null;
		}
	}
}
