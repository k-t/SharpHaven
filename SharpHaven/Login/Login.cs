using System;
using System.Threading;
using System.Threading.Tasks;
using NLog;
using SharpHaven.Game;
using SharpHaven.Net;

namespace SharpHaven.Login
{
	public class Login
	{
		private static readonly Logger Log = LogManager.GetCurrentClassLogger();

		public string UserName
		{
			get { return App.Config.UserName; }
			set { App.Config.UserName = value; }
		}

		public string Password
		{
			get;
			set;
		}

		public bool HasToken
		{
			get { return Token != null; }
		}

		private byte[] Token
		{
			get { return App.Config.AuthToken; }
			set { App.Config.AuthToken = value; }
		}

		public void ForgetToken()
		{
			Token = null;
		}

		public bool Remember
		{
			get;
			set;
		}

		public event Action<string> Progress;
		public event Action<LoginResult> Finished;

		public void LoginAsync()
		{
			// create state in the calling thread
			var state = new GameState();
			var authenticate = (Token != null)
				? new Func<AuthResult>(() => Authenticate(UserName, Token))
				: new Func<AuthResult>(() => Authenticate(UserName, Password));
			RunAsync(() =>
			{
				try
				{
					ReportProgress("Authenticating...");

					var result = authenticate();
					if (result.Cookie == null)
					{
						App.QueueOnMainThread(() => Finished.Raise(new LoginResult(result.Error)));
						return;
					}

					ReportProgress("Connecting...");

					var session = CreateSession(state, UserName, result.Cookie);
					session.Start();

					Log.Info("{0} logged in successfully", UserName);
					Finish(new LoginResult(session));
				}
				catch (AuthException ex)
				{
					Log.Error("Authentication error", (Exception)ex);
					Finish(new LoginResult(ex.Message));
				}
				catch (ConnectionException ex)
				{
					Log.Error("Connection error ({0}) {1}", (byte)ex.Error, ex.Message);
					Finish(new LoginResult(ex.Message));
				}
				catch (Exception ex)
				{
					Log.Error("Unexpected login error", ex);
					Finish(new LoginResult(ex.Message));
				}
			});
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
					UserName = userName;
					Token = Remember ? authClient.GetToken() : null;
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
				ForgetToken();
				return new AuthResult("Invalid save");
			}
		}

		private GameSession CreateSession(GameState state, string userName, byte[] cookie)
		{
			var connSettings = new ConnectionSettings
			{
				Host = App.Config.GameHost,
				Port = App.Config.GamePort,
				UserName = userName,
				Cookie = cookie
			};
			var game = new Connection(connSettings);
			return new GameSession(state, game);
		}

		private static void RunAsync(Action action)
		{
			Task.Factory.StartNew(
				action,
				CancellationToken.None,
				TaskCreationOptions.None,
				TaskScheduler.Default);
		}

		private void ReportProgress(string message)
		{
			App.QueueOnMainThread(() => Progress.Raise(message));
		}

		private void Finish(LoginResult result)
		{
			App.QueueOnMainThread(() => Finished.Raise(result));
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
