using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using NLog;
using SharpHaven.Game;
using SharpHaven.Network;

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

		public Task<LoginResult> LoginAsync()
		{
			// create state in the calling thread
			var state = new GameState();

			var asyncContext = AsyncOperationManager.CreateOperation(null);
			var authenticate = (Token != null)
				? new Func<AuthResult>(() => Authenticate(UserName, Token))
				: new Func<AuthResult>(() => Authenticate(UserName, Password));

			return RunAsync(() =>
			{
				try
				{
					asyncContext.Post(_ => Progress.Raise("Authenticating..."), null);

					var result = authenticate();
					if (result.Cookie == null)
						return new LoginResult(result.Error);

					asyncContext.Post(_ => Progress.Raise("Connecting..."), null);

					var session = CreateSession(state, UserName, result.Cookie);
					session.Start();

					Log.Info("{0} logged in successfully", UserName);
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
			return new GameSession(state, connSettings);
		}

		private static Task<T> RunAsync<T>(Func<T> func)
		{
			return Task.Factory.StartNew(
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
