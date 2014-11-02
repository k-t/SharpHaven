using System;
using System.ComponentModel;

namespace MonoHaven.Utils
{
	public static class AsyncOperationExtensions
	{
		public static void PostEvent<T>(this AsyncOperation operation,
			EventHandler<T> eventHandler, object sender, T args)
			where T : EventArgs
		{
			operation.Post(_ => {
				if (eventHandler != null)
					eventHandler(sender, args);
			}, null);
		}
	}
}
