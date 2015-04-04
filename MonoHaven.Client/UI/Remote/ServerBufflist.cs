﻿using MonoHaven.UI.Widgets;

namespace MonoHaven.UI.Remote
{
	public class ServerBufflist : ServerWidget
	{
		private Bufflist widget;

		public ServerBufflist(ushort id, ServerWidget parent) : base(id, parent)
		{
		}

		public override Widget Widget
		{
			get { return widget; }
		}

		public static ServerWidget Create(ushort id, ServerWidget parent)
		{
			return new ServerBufflist(id, parent);
		}

		protected override void OnInit(object[] args)
		{
			widget = new Bufflist(Parent.Widget, Parent.Session.State);
		}
	}
}
