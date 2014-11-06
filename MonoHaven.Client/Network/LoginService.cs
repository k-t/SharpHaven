using System;
using System.Threading;
using System.Threading.Tasks;
using NLog;

namespace MonoHaven.Network
{
	public class LoginService
	{
		private static readonly Logger Log = LogManager.GetCurrentClassLogger();

		private readonly LoginSettings settings;

		public LoginService(LoginSettings settings)
		{
			this.settings = settings;
		}

		public async Task<LoginResult> LoginAsync(
			string userName,
			string password,
			Action<string> reportStatus)
		{
			try
			{
				reportStatus("Authenticating...");

				var cookie = await RunAsync(() => Authenticate(userName, password));
				if (cookie == null)
					return new LoginResult("Username or password incorrect");

				reportStatus("Connecting...");

				var connection = CreateConnection(userName, cookie);
				await RunAsync(connection.Open);

				Log.Info("<{0}> logged in successfully", userName);
				return new LoginResult();
			}
			catch (AuthException ex)
			{
				Log.Error("Authentication error", (Exception)ex);
				return new LoginResult(ex.Message);
			}
			catch (ConnectionException ex)
			{
				Log.Error("Connection error ({0})", (byte)ex.Error);
				return new LoginResult(ex.Message);
			}
			catch (Exception ex)
			{
				Log.Error("Unexpected login error", ex);
				return new LoginResult(ex.Message);
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

		private Connection CreateConnection(string userName, byte[] cookie)
		{
			var settings = new ConnectionSettings
			{
				Host = this.settings.GameHost,
				Port = this.settings.GamePort,
				UserName = userName,
				Cookie = cookie
			};
			return new Connection(settings);
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
