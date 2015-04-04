﻿using System.Drawing;
using MonoHaven.UI.Widgets;

namespace MonoHaven.UI.Remote
{
	public class ServerChat : ServerWidget
	{
		private Chat widget;

		public ServerChat(ushort id, ServerWidget parent) : base(id, parent)
		{
			SetHandler("log", Log);
			// TODO: SetHandler("focusme", ...)
		}

		public override Widget Widget
		{
			get { return widget; }
		}

		public static ServerWidget Create(ushort id, ServerWidget parent)
		{
			return new ServerChat(id, parent);
		}

		protected override void OnInit(object[] args)
		{
			var title = (string)args[0];
			var closable = (args.Length > 1) && ((int)args[1]) != 0;

			widget = new Chat(Parent.Widget, title, closable);
			widget.MessageOut += (msg) => SendMessage("msg", msg);
		}

		private void Log(object[] args)
		{
			var text = (string)args[0];
			var color = args.Length > 1 ? (Color?)args[1] : null;
			var alert = args.Length > 2 ? (int?)args[2] : null;
			widget.AddMessage(text, color);
		}
	}
}
