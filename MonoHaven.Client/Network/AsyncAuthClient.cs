using System;
using System.Threading.Tasks;

namespace MonoHaven.Network
{
	public class AsyncAuthClient
	{
		private readonly string host;
		private readonly int port;
		private readonly CallbackDispatcher dispatcher;

		public AsyncAuthClient(string host, int port, CallbackDispatcher dispatcher)
		{
			this.host = host;
			this.port = port;
			this.dispatcher = dispatcher;
		}

		public event EventHandler<AuthProgressEventArgs> ProgressChanged;
		public event EventHandler<AuthResultEventArgs> Completed;

		public void Authenticate(string userName, string password)
		{
			Task.Factory.StartNew(() => {
				AuthClient authClient = null;
				try
				{
					authClient = new AuthClient(host, port);
					byte[] cookie;
					ChangeProgress("Authenticating...");
					authClient.Connect();
					authClient.BindUser(userName);
					if (authClient.TryPassword(password, out cookie))
					{
						ChangeProgress("Connecting...");
						Complete(cookie);
					}
					else
						Complete("Username or password incorrect");
				}
				catch (Exception e)
				{
					Complete(e.Message);
				}
				finally
				{
					if (authClient != null)
						authClient.Dispose();
				}
			});
		}

		private void Complete(byte[] cookie)
		{
			Complete(new AuthResultEventArgs(cookie));
		}

		private void Complete(string error)
		{
			Complete(new AuthResultEventArgs(error));
		}

		private void Complete(AuthResultEventArgs args)
		{
			dispatcher.Dispatch(Completed, this, args);
		}

		private void ChangeProgress(string statusText)
		{
			var args = new AuthProgressEventArgs(statusText);
			dispatcher.Dispatch(ProgressChanged, this, args);
		}
	}
}
