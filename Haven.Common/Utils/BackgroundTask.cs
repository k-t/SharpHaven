using System;
using System.Threading;
using NLog;

namespace Haven.Utils
{
	public abstract class BackgroundTask
	{
		private static readonly Logger Log = LogManager.GetCurrentClassLogger();

		private volatile bool isCancelled;
		private readonly Thread thread;
		
		protected BackgroundTask(string name)
		{
			thread = new Thread(Start);
			thread.IsBackground = true;
			thread.Name = name;
		}

		public event EventHandler Finished;

		protected bool IsCancelled
		{
			get { return isCancelled; }
		}

		public void Run()
		{
			thread.Start();
		}

		public void Stop()
		{
			// TODO: it's assumed that task acts nicely but what if it's not the case?
			isCancelled = true;
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
				Log.Error(ex, "Unhandled exception within task");
			}
			finally
			{
				Finished.Raise(this, EventArgs.Empty);
				Log.Info("Task stopped");
			}
		}
	}
}
