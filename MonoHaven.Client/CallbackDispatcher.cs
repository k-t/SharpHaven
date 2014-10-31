using System;
using System.Threading;
using System.Windows.Threading;

namespace MonoHaven
{
	public class CallbackDispatcher
	{
		private readonly SynchronizationContext syncContext;

		public CallbackDispatcher()
		{
			syncContext = new DispatcherSynchronizationContext();
			SynchronizationContext.SetSynchronizationContext(syncContext);
		}

		public void Dispatch<T>(EventHandler<T> handler, object sender, T args)
			where T : EventArgs
		{
			if (handler != null)
				syncContext.Post(_ => handler(sender, args), null);
		}
	}
}
