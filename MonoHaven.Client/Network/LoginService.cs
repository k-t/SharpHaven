using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace MonoHaven.Network
{
	public class LoginService
	{
		private readonly LoginOptions options;

		public LoginService(LoginOptions options)
		{
			this.options = options;
		}

		public event EventHandler<LoginStatusEventArgs> StatusChanged;

		public LoginResult Login(string userName, string password)
		{
			return Login(userName, password, null);
		}

		public async Task<LoginResult> LoginAsync(string userName, string password)
		{
			var operation = AsyncOperationManager.CreateOperation(null);
			var result = await Task<LoginResult>.Factory.StartNew(
				() => Login(userName, password, operation));
			return result;
		}

		private LoginResult Login(string userName, string password, AsyncOperation operation)
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
