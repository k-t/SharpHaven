using System.Threading;

namespace MonoHaven.Network
{
	public abstract class GameConnectionWorker
	{
		private readonly GameConnection connection;
		private readonly CancellationTokenSource tokenSource;
		private readonly CancellationToken token;
		private readonly Thread thread;
		
		protected GameConnectionWorker(string name, GameConnection connection)
		{
			this.connection = connection;

			thread = new Thread(Run);
			thread.IsBackground = true;
			thread.Name = name;

			tokenSource = new CancellationTokenSource();
			token = tokenSource.Token;
		}

		protected GameConnection Connection
		{
			get { return connection; }
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
