﻿using System.Drawing;
using SharpHaven.UI.Widgets;

namespace SharpHaven.UI.Remote
{
	public class ServerCalendar : ServerWidget
	{
		private Calendar widget;

		public ServerCalendar(ushort id, ServerWidget parent) : base(id, parent)
		{
		}

		public override Widget Widget
		{
			get { return null; }
		}

		public static ServerWidget Create(ushort id, ServerWidget parent)
		{
			return new ServerCalendar(id, parent);
		}

		protected override void OnInit(Point position, object[] args)
		{
			widget = Session.State.Screen.Calendar;
			widget.Visible = true;
			widget.State = Session.State;
		}

		protected override void OnDestroy()
		{
			widget.Visible = false;
			widget.State = null;
		}
	}
}