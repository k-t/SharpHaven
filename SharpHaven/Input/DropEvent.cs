﻿using System.Drawing;

namespace SharpHaven.Input
{
	public class DropEvent : MouseEvent
	{
		private readonly object data;

		public DropEvent(Point mousePosition, object data)
			: base(mousePosition)
		{
			this.data = data;
		}

		public object Data
		{
			get { return data; }
		}
	}
}