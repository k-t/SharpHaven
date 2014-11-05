using System;
using System.Threading;
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

		public async Task<LoginResult> LoginAsync(string userName, string password)
		{
			try
			{
				ChangeStatus("Authenticating...");

				var cookie = await RunAsync(() => Authenticate(userName, password));
				if (cookie == null)
					return new LoginResult("Username or password incorrect");

				ChangeStatus("Connecting...");

				var connection = CreateConnection(userName, cookie);
				await RunAsync(connection.Open);

				return new LoginResult();
			}
			catch (Exception e)
			{
				return new LoginResult(e.Message);
			}
		}

		private byte[] Authenticate(string userName, string password)
		{
			byte[] cookie;
			using (var authClient = new AuthClient(settings.AuthHost, settings.AuthPort))
			{
				authClient.Connect();
				authClient.BindUser(userName);
				return authClient.TryPassword(password, out cookie) ? cookie : null;
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

		private void ChangeStatus(string status)
		{
			var args = new LoginStatusEventArgs(status);
			if (StatusChanged != null)
				StatusChanged(this, args);
		}

		private static Task RunAsync(Action action)
		{
			return Task.Factory.StartNew(
				action,
				CancellationToken.None,
				TaskCreationOptions.None,
				TaskScheduler.Default);
		}

		private static Task<T> RunAsync<T>(Func<T> func)
		{
			return Task<T>.Factory.StartNew(
				func,
				CancellationToken.None,
				TaskCreationOptions.None,
				TaskScheduler.Default);
		}
	}
}
