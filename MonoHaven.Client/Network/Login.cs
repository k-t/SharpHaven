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

		public LoginResult DoLogin(string userName, string password)
		{
			return DoLogin(userName, password, null);
		}

		public async Task<LoginResult> DoLoginAsync(string userName, string password)
		{
			var operation = AsyncOperationManager.CreateOperation(null);
			var result = await Task<LoginResult>.Factory.StartNew(
				() => DoLogin(userName, password, operation));
			return result;
		}

		private LoginResult DoLogin(string userName, string password, AsyncOperation operation)
		{
			byte[] cookie;
			try
			{
				ChangeStatus("Authenticating...", operation);
				if (!Authenticate(userName, password, out cookie))
					return new LoginResult("Username or password incorrect");

				ChangeStatus("Connecting...", operation);
				// TODO: connect to game server
				return new LoginResult(cookie);
			}
			catch (Exception e)
			{
				return new LoginResult(e.Message);
			}
		}

		private bool Authenticate(string userName, string password, out byte[] cookie)
		{
			using (var authClient = new AuthClient(options.AuthHost, options.AuthPort))
			{
				authClient.Connect();
				authClient.BindUser(userName);
				return authClient.TryPassword(password, out cookie);
			}
		}

		private void ChangeStatus(string status, AsyncOperation operation)
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
