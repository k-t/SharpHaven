using System;
using System.ComponentModel;
using System.Threading.Tasks;
using MonoHaven.Utils;

namespace MonoHaven.Network
{
	public class AsyncAuthClient
	{
		private readonly string host;
		private readonly int port;

		public AsyncAuthClient(string host, int port)
		{
			this.host = host;
			this.port = port;
		}

		public event EventHandler<AuthStatusEventArgs> StatusChanged;

		public async Task<AuthResult> Authenticate(string userName, string password)
		{
			var operation = AsyncOperationManager.CreateOperation(null);
			var result = await Task<AuthResult>.Factory.StartNew(() => {
				AuthClient authClient = null;
				try
				{
					authClient = new AuthClient(host, port);
					byte[] cookie;
					ChangeProgress(operation, "Authenticating...");
					authClient.Connect();
					authClient.BindUser(userName);
					if (authClient.TryPassword(password, out cookie))
					{
						ChangeProgress(operation, "Connecting...");
						return new AuthResult(cookie);
					}
					else
						return new AuthResult("Username or password incorrect");
				}
				catch (Exception e)
				{
					return new AuthResult(e.Message);
				}
				finally
				{
					if (authClient != null)
						authClient.Dispose();
				}
			});
			return result;
		}

		private void ChangeProgress(AsyncOperation operation, string status)
		{
			operation.PostEvent(StatusChanged, this, new AuthStatusEventArgs(status));
		}
	}
}
