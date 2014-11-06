using System.Threading;

namespace MonoHaven
{
	public abstract class BackgroundTask
	{
		private readonly CancellationTokenSource tokenSource;
		private readonly CancellationToken token;
		private readonly Thread thread;
		
		protected BackgroundTask(string name)
		{
			thread = new Thread(Run);
			thread.IsBackground = true;
			thread.Name = name;

			tokenSource = new CancellationTokenSource();
			token = tokenSource.Token;
		}

		protected bool IsCancelled
		{
			get { return token.IsCancellationRequested; }
		}

		public void Start()
		{
			thread.Start();
		}

		public void Stop()
		{
			tokenSource.Cancel();
		}

		protected abstract void Run();
	}
}
