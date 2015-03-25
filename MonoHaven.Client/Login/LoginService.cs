using System;
using System.Threading;
using System.Threading.Tasks;
using MonoHaven.Game;
using MonoHaven.Network;
using NLog;

namespace MonoHaven.Login
{
	public class LoginService
	{
		private static readonly Logger Log = LogManager.GetCurrentClassLogger();

		public Task<LoginResult> LoginAsync(
			string userName,
			byte[] token,
			Action<string> reportStatus)
		{
			return LoginAsync(userName, () => Authenticate(userName, token), reportStatus);
		}

		public Task<LoginResult> LoginAsync(
			string userName,
			string password,
			Action<string> reportStatus)
		{
			return LoginAsync(userName, () => Authenticate(userName, password), reportStatus);
		}

		private async Task<LoginResult> LoginAsync(
			string userName,
			Func<AuthResult> authFunc,
			Action<string> reportStatus)
		{
			try
			{
				reportStatus("Authenticating...");

				var result = await RunAsync(authFunc);
				if (result.Cookie == null)
					return new LoginResult(result.Error);

				reportStatus("Connecting...");

				var session = CreateSession(userName, result.Cookie);
				await RunAsync(session.Start);

				Log.Info("{0} logged in successfully", userName);
				return new LoginResult(session);
			}
			catch (AuthException ex)
			{
				Log.Error("Authentication error", (Exception)ex);
				return new LoginResult(ex.Message);
			}
			catch (ConnectionException ex)
			{
				Log.Error("Connection error ({0}) {1}", (byte)ex.Error, ex.Message);
				return new LoginResult(ex.Message);
			}
			catch (Exception ex)
			{
				Log.Error("Unexpected login error", ex);
				return new LoginResult(ex.Message);
			}
		}

		private AuthResult Authenticate(string userName, string password)
		{
			byte[] cookie;
			using (var authClient = new AuthClient(App.Config.AuthHost, App.Config.AuthPort))
			{
				authClient.Connect();
				authClient.BindUser(userName);
				if (authClient.TryPassword(password, out cookie))
				{
					App.Config.UserName = userName;
					App.Config.AuthToken = authClient.GetToken();
					return new AuthResult(cookie);
				}
				return new AuthResult("Username or password incorrect");
			}
		}

		private AuthResult Authenticate(string userName, byte[] token)
		{
			byte[] cookie;
			using (var authClient = new AuthClient(App.Config.AuthHost, App.Config.AuthPort))
			{
				authClient.Connect();
				authClient.BindUser(userName);
				if (authClient.TryToken(token, out cookie))
					return new AuthResult(cookie);
				App.Config.AuthToken = null;
				return new AuthResult("Invalid save");
			}
		}

		private GameSession CreateSession(string userName, byte[] cookie)
		{
			var connSettings = new ConnectionSettings
			{
				Host = App.Config.GameHost,
				Port = App.Config.GamePort,
				UserName = userName,
				Cookie = cookie
			};
			return new GameSession(connSettings);
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

		private class AuthResult
		{
			private readonly byte[] cookie;
			private readonly string error;

			public AuthResult(byte[] cookie)
			{
				this.cookie = cookie;
			}

			public AuthResult(string error)
			{
				this.error = error;
			}

			public byte[] Cookie
			{
				get { return cookie; }
			}

			public string Error
			{
				get { return error; }
			}
		}
	}
}
