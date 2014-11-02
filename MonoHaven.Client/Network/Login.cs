using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace MonoHaven.Network
{
	public class Login
	{
		private readonly LoginOptions options;

		public Login(LoginOptions options)
		{
			this.options = options;
		}

		public event EventHandler<LoginStatusEventArgs> StatusChanged;

		public async Task<LoginResult> DoLoginAsync(string userName, string password)
		{
			var operation = AsyncOperationManager.CreateOperation(null);
			var result = await Task<LoginResult>.Factory.StartNew(
				() => Authenticate(operation, userName, password));
			return result;
		}

		private LoginResult Authenticate(AsyncOperation operation, string userName, string password)
		{
			AuthClient authClient = null;
			try
			{
				authClient = new AuthClient(options.AuthHost, options.AuthPort);
				byte[] cookie;
				ChangeStatus(operation, "Authenticating...");
				authClient.Connect();
				authClient.BindUser(userName);
				if (authClient.TryPassword(password, out cookie))
				{
					ChangeStatus(operation, "Connecting...");
					return new LoginResult(cookie);
				}
				else
					return new LoginResult("Username or password incorrect");
			}
			catch (Exception e)
			{
				return new LoginResult(e.Message);
			}
			finally
			{
				if (authClient != null)
					authClient.Dispose();
			}
		}

		private void ChangeStatus(AsyncOperation operation, string status)
		{
			var args = new LoginStatusEventArgs(status);
			if (operation != null)
				operation.Post(_ => OnStatusChanged(args), null);
			else
				OnStatusChanged(args);
		}

		private void OnStatusChanged(LoginStatusEventArgs args)
		{
			if (StatusChanged != null)
				StatusChanged(this, args);
		}
	}
}
