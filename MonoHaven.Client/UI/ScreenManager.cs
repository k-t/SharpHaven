using System;

namespace MonoHaven.UI
{
	public class ScreenManager : IDisposable
	{
		private LoginScreen loginScreen;
		private GameSessionScreen gameSessionScreen;

		public Screen CurrentScreen
		{
			get;
			private set;
		}

		public void Init(IScreenHost host)
		{
			this.loginScreen = new LoginScreen(host);
			this.gameSessionScreen = new GameSessionScreen(host);

			CurrentScreen = loginScreen;

			this.loginScreen.LoggedIn += (sender, args) => CurrentScreen = this.gameSessionScreen;
		}

		public void Dispose()
		{
			loginScreen.Dispose();
			gameSessionScreen.Dispose();
		}
	}
}
