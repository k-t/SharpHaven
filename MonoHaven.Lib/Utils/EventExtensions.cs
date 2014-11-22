#region License
// Copyright (c) 2014 Marat Vildanov <marat.vildanov@gmail.com>
// Distributed under the MIT License.
// (See accompanying LICENSE file or copy at http://opensource.org/licenses/MIT)
#endregion

using System;

namespace MonoHaven.Utils
{
	public static class EventExtensions
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
