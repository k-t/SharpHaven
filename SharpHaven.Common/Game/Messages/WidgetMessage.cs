﻿namespace SharpHaven.Game.Messages
{
	public class WidgetMessage
	{
		public ushort WidgetId { get; set; }

		public string Name { get; set; }

		public object[] Args { get; set; }
	}
}