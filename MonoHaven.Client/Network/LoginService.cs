using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace MonoHaven.Network
{
	public class LoginService
	{
		private readonly LoginSettings settings;

		public LoginService(LoginSettings settings)
		{
			this.settings = settings;
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
				var connection = CreateConnection(userName, cookie);
				connection.Open();
				return new LoginResult();
			}
			catch (Exception e)
			{
				return new LoginResult(e.Message);
			}
		}

		private bool Authenticate(string userName, string password, out byte[] cookie)
		{
			using (var authClient = new AuthClient(settings.AuthHost, settings.AuthPort))
			{
				authClient.Connect();
				authClient.BindUser(userName);
				return authClient.TryPassword(password, out cookie);
			}
		}

		private GameConnection CreateConnection(string userName, byte[] cookie)
		{
			var settings = new ConnectionSettings
			{
				Host = this.settings.GameHost,
				Port = this.settings.GamePort,
				UserName = userName,
				Cookie = cookie
			};
			return new GameConnection(settings);
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
