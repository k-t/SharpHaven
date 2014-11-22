#region License
// Copyright (c) 2014 Marat Vildanov <marat.vildanov@gmail.com>
// Distributed under the MIT License.
// (See accompanying LICENSE file or copy at http://opensource.org/licenses/MIT)
#endregion

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

				var session = CreateSession(userName, cookie);
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

		private GameSession CreateSession(string userName, byte[] cookie)
		{
			var connSettings = new ConnectionSettings
			{
				Host = settings.GameHost,
				Port = settings.GamePort,
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
	}
}
