using System;
using System.Threading;
using System.Threading.Tasks;
using Haven;
using Haven.Net;
using NLog;

namespace SharpHaven.Login
{
	public class Login
	{
		private static readonly Logger Log = LogManager.GetCurrentClassLogger();

		private readonly GameClient client;

		public Login(GameClient client)
		{
			this.client = client;
		}

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

		public event Action<LoginResult> Finished;

		public void LoginAsync()
		{
			var authenticate = (Token != null)
				? new Action(() => Authenticate(UserName, Token))
				: new Action(() => Authenticate(UserName, Password));
			RunAsync(() =>
			{
				try
				{
					authenticate();
					Log.Info("{0} logged in successfully", UserName);
					Finish(LoginResult.Success());
				}
				catch (AuthException ex)
				{
					Log.Error(ex, "Authentication error");
					Finish(LoginResult.Fail(ex.Message));
				}
				catch (NetworkException ex)
				{
					Log.Error("Connection error ({0}) {1}", (byte)ex.Error, ex.Message);
					Finish(LoginResult.Fail(ex.Message));
				}
				catch (Exception ex)
				{
					Log.Error(ex, "Unexpected login error");
					Finish(LoginResult.Fail(ex.Message));
				}
			});
		}

		private void Authenticate(string userName, string password)
		{
			var result = client.Authenticate(userName, password, Remember);
			if (result.IsSuccessful)
			{
				if (Remember)
					Token = result.Token;
				return;
			}
			throw new AuthException("Username or password incorrect");
		}

		private void Authenticate(string userName, byte[] token)
		{
			var result = client.Authenticate(userName, token);
			if (result.IsSuccessful)
			{
				return;
			}
			ForgetToken();
			throw new AuthException("Invalid save");
		}

		private static void RunAsync(Action action)
		{
			Task.Factory.StartNew(
				action,
				CancellationToken.None,
				TaskCreationOptions.None,
				TaskScheduler.Default);
		}

		private void Finish(LoginResult result)
		{
			App.QueueOnMainThread(() => Finished.Raise(result));
		}
	}
}
