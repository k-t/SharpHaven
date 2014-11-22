#region License
// Copyright (c) 2014 Marat Vildanov <marat.vildanov@gmail.com>
// Distributed under the MIT License.
// (See accompanying LICENSE file or copy at http://opensource.org/licenses/MIT)
#endregion

using System;
using System.Threading;
using MonoHaven.Utils;
using NLog;

namespace MonoHaven
{
	public abstract class BackgroundTask
	{
		private static readonly Logger log = LogManager.GetCurrentClassLogger();

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

		public EventHandler Finished;

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
			log.Info("Task started");
			try
			{
				OnStart();
			}
			catch (Exception ex)
			{
				log.Error("Unhandled exception within task", ex);
			}
			finally
			{
				Finished.Raise(this, EventArgs.Empty);
				log.Info("Task stopped");
			}
		}
	}
}
