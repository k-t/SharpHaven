﻿namespace SharpHaven.Game.Events
{
	public class ResourceLoadEvent
	{
		public ushort ResourceId
		{
			get;
			set;
		}

		public string Name
		{
			get;
			set;
		}

		public ushort Version
		{
			get;
			set;
		}
	}
}
