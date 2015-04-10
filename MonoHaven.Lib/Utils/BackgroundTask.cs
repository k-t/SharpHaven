using System;
using System.Threading;
using NLog;

namespace SharpHaven.Utils
{
	public abstract class BackgroundTask
	{
		private static readonly Logger Log = LogManager.GetCurrentClassLogger();

		private readonly CancellationTokenSource tokenSource;
		private readonly CancellationToken token;
		private readonly Thread thread;
		
		protected BackgroundTask(string name)
		{
			thread = new Thread(Start);
			thread.IsBackground = true;
			thread.Name = name;

			tokenSource = new CancellationTokenSource();
			token = tokenSource.Token;
		}

		public event EventHandler Finished;

		protected bool IsCancelled
		{
			get { return token.IsCancellationRequested; }
		}

		public void Run()
		{
			thread.Start();
		}

		public void Stop()
		{
			// TODO: it's assumed that task acts nicely but what if it's not the case?
			tokenSource.Cancel();
		}

		protected abstract void OnStart();

		private void Start()
		{
			Log.Info("Task started");
			try
			{
				OnStart();
			}
			catch (Exception ex)
			{
				Log.Error("Unhandled exception within task", ex);
			}
			finally
			{
				Finished.Raise(this, EventArgs.Empty);
				Log.Info("Task stopped");
			}
		}
	}
}
