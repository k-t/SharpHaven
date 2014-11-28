﻿using System;

namespace MonoHaven
{
	public static class EventHelper
	{
		public static void Raise(this Action action)
		{
			if (action != null)
				action();
		}

		public static void Raise<T>(this Action<T> action, T arg)
		{
			if (action != null)
				action(arg);
		}

		public static void Raise(this EventHandler handler, object sender, EventArgs args)
		{
			if (handler != null)
				handler(sender, args);
		}

		public static void Raise<T>(this EventHandler<T> handler, object sender, T args)
			where T : EventArgs
		{
			if (handler != null)
				handler(sender, args);
		}
	}
}