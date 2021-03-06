﻿using Haven;
using SharpHaven.UI.Widgets;

namespace SharpHaven.UI.Remote
{
	public class ServerGiveButton : ServerWidget
	{
		private GiveButton widget;

		public ServerGiveButton(ushort id, ServerWidget parent)
			: base(id, parent)
		{
		}

		public override Widget Widget
		{
			get { return widget; }
		}

		public static ServerWidget Create(ushort id, ServerWidget parent)
		{
			return new ServerGiveButton(id, parent);
		}

		protected override void OnInit(Point2D position, object[] args)
		{
			widget = new GiveButton(Parent.Widget);
			widget.Move(position);
		}
	}
}
