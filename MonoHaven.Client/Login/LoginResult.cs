using SharpHaven.Game;

namespace SharpHaven.Login
{
	public class LoginResult
	{
		private readonly string error;
		private readonly GameSession session;

		public LoginResult(GameSession session)
		{
			this.session = session;
		}

		public LoginResult(string error)
		{
			this.error = error;
		}

		public bool IsSuccessful
		{
			get { return session != null; }
		}

		public string Error
		{
			get { return error; }
		}

		public GameSession Session
		{
			get { return session; }
		}
	}
}
